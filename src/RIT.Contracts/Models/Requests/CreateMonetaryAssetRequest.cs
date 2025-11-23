namespace RIT.Contracts.Models.Requests;

public class CreateMonetaryAssetRequest
{
    public required string Type { get; set; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "RUB";
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
}
