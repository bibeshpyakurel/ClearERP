namespace ClearErp.Domain.Common;

public sealed class UnauthorizedException(string message) : Exception(message);
