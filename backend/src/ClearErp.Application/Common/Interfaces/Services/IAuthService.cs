namespace ClearErp.Application.Common.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
