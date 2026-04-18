using System.Net.Mail;

namespace ClearErp.Domain.Common;

public static class Guard
{
    public static void AgainstEmpty(Guid value, string name)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException($"{name} is required.");
        }
    }

    public static void AgainstNullOrWhiteSpace(string? value, string name, int maxLength = int.MaxValue)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{name} is required.");
        }

        if (value.Length > maxLength)
        {
            throw new DomainException($"{name} must not exceed {maxLength} characters.");
        }
    }

    public static void AgainstNegative(int value, string name)
    {
        if (value < 0)
        {
            throw new DomainException($"{name} must be zero or greater.");
        }
    }

    public static void AgainstNegative(decimal value, string name)
    {
        if (value < 0)
        {
            throw new DomainException($"{name} must be zero or greater.");
        }
    }

    public static void AgainstZeroOrNegative(int value, string name)
    {
        if (value <= 0)
        {
            throw new DomainException($"{name} must be greater than zero.");
        }
    }

    public static void AgainstInvalidEmail(string? value, string name)
    {
        AgainstNullOrWhiteSpace(value, name);

        try
        {
            _ = new MailAddress(value!);
        }
        catch (FormatException)
        {
            throw new DomainException($"{name} must be a valid email address.");
        }
    }
}
