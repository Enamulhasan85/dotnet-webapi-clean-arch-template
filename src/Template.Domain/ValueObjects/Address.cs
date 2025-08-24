using System.Text.RegularExpressions;

namespace Template.Domain.ValueObjects;

public record Address
{
    public string Street { get; init; }
    public string City { get; init; }
    public string State { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty", nameof(street));
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

        Street = street.Trim();
        City = city.Trim();
        State = state?.Trim() ?? string.Empty;
        PostalCode = postalCode.Trim();
        Country = country?.Trim() ?? "USA";
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State} {PostalCode}, {Country}";
    }
}
