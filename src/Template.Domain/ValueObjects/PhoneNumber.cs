using System.Text.RegularExpressions;

namespace Template.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);
    
    public string Value { get; init; }
    public string CountryCode { get; init; }
    public string Number { get; init; }

    public PhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        var cleaned = phoneNumber.Trim().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        
        if (!PhoneRegex.IsMatch(cleaned))
            throw new ArgumentException("Invalid phone number format", nameof(phoneNumber));

        Value = cleaned;
        
        // Simple parsing for demonstration
        if (cleaned.StartsWith("+1"))
        {
            CountryCode = "+1";
            Number = cleaned[2..];
        }
        else if (cleaned.StartsWith("+"))
        {
            var countryCodeEnd = Math.Min(4, cleaned.Length);
            CountryCode = cleaned[..countryCodeEnd];
            Number = cleaned[countryCodeEnd..];
        }
        else
        {
            CountryCode = "+1"; // Default to US
            Number = cleaned;
        }
    }

    public override string ToString() => Value;
}
