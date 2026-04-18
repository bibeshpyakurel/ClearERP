using ClearErp.Domain.Entities;

namespace ClearErp.Application.Common.Interfaces.Security;

public interface IJwtTokenGenerator
{
    (string AccessToken, DateTime ExpiresAtUtc) GenerateToken(User user, IReadOnlyCollection<string> roles);
}
