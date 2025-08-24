using System.Text.RegularExpressions;

namespace Template.Domain.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; init; }
    public string LocalPart { get; init; }
    public string Domain { get; init; }

    public Email(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new ArgumentException("Email address cannot be empty", nameof(emailAddress));

        var trimmed = emailAddress.Trim().ToLowerInvariant();
        
        if (!EmailRegex.IsMatch(trimmed))
            throw new ArgumentException("Invalid email address format", nameof(emailAddress));

        if (trimmed.Length > 254)
            throw new ArgumentException("Email address is too long", nameof(emailAddress));

        Value = trimmed;
        var parts = trimmed.Split('@');
        LocalPart = parts[0];
        Domain = parts[1];
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
