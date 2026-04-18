using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ClearErp.Api.Middleware;
using ClearErp.Application.Common.Interfaces;
using ClearErp.Application;
using ClearErp.Infrastructure;
using ClearErp.Infrastructure.Persistence;
using Serilog;
using System.Text;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation(options =>
{
    options.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>(
        name: "postgres",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "ready"]);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "The input was invalid." : error.ErrorMessage)
                    .ToArray());

        var response = new ApiErrorResponse(
            StatusCodes.Status400BadRequest,
            "Validation failed",
            "One or more validation errors occurred.",
            context.HttpContext.TraceIdentifier,
            Type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Instance: context.HttpContext.Request.Path.Value,
            Errors: errors);

        return new BadRequestObjectResult(response) { ContentTypes = { "application/problem+json" } };
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:5173"];

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var issuer = jwtSettings["Issuer"] ?? "ClearErp";
var audience = jwtSettings["Audience"] ?? "ClearErp.Client";
var key = jwtSettings["Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClearERP API",
        Version = "v1",
        Description = "Inventory, purchasing, reporting, and audit API for the ClearERP system. Use the Auth module first to obtain a JWT, then authorize Swagger to exercise the protected ERP workflows end to end."
    });

    options.TagActionsBy(api =>
    {
        if (!string.IsNullOrWhiteSpace(api.GroupName))
        {
            return [api.GroupName];
        }

        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return !string.IsNullOrWhiteSpace(controllerName) ? [controllerName] : ["API"];
    });
    options.OrderActionsBy(api => $"{api.GroupName}_{api.RelativePath}");
    options.SupportNonNullableReferenceTypes();

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Paste the JWT access token returned by POST /api/auth/login. Format: Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [securityScheme] = Array.Empty<string>()
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePages(async statusCodeContext =>
{
    var response = statusCodeContext.HttpContext.Response;
    if (response.HasStarted || response.ContentLength.HasValue || !string.IsNullOrEmpty(response.ContentType))
    {
        return;
    }

    var title = response.StatusCode switch
    {
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        _ => "Request failed"
    };

    var detail = response.StatusCode switch
    {
        StatusCodes.Status401Unauthorized => "Authentication is required to access this resource.",
        StatusCodes.Status403Forbidden => "You do not have permission to access this resource.",
        StatusCodes.Status404NotFound => "The requested resource was not found.",
        _ => "The request could not be completed."
    };

    response.ContentType = "application/problem+json";
    var payload = new ApiErrorResponse(
        response.StatusCode,
        title,
        detail,
        statusCodeContext.HttpContext.TraceIdentifier,
        Type: $"https://tools.ietf.org/html/rfc9110#section-15.{(response.StatusCode >= 500 ? "6" : "5")}.{response.StatusCode - (response.StatusCode >= 500 ? 499 : 399)}",
        Instance: statusCodeContext.HttpContext.Request.Path.Value);

    await response.WriteAsJsonAsync(payload, new System.Text.Json.JsonSerializerOptions
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
    });
});

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = new
        {
            status = report.Status.ToString(),
            duration = report.TotalDuration,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration,
                tags = e.Value.Tags
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
});

app.MapGet("/api/user-context", [Microsoft.AspNetCore.Authorization.Authorize] (ICurrentUserService currentUserService) =>
    Results.Ok(new
    {
        currentUserService.UserId,
        currentUserService.Email,
        currentUserService.Roles,
        currentUserService.IsAuthenticated
    }))
    .WithName("GetUserContext")
    .WithTags("Auth")
    .WithSummary("Returns the authenticated user context extracted from the JWT.")
    .WithDescription("Convenience endpoint for frontend session bootstrapping after authentication.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized);

app.Run();

public partial class Program;
