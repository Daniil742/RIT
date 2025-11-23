namespace RIT.Contracts.Models;

/// <summary>
/// Денежный актив в банке.
/// </summary>
public class BankAssetDto : MonetaryAssetDto
{
    public required string BankInfo { get; set; }
    public string? RawBankName { get; set; }
    public string? RawAccountNumber { get; set; }
}
