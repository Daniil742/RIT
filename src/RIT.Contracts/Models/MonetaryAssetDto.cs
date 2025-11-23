namespace RIT.Contracts.Models;

/// <summary>
/// Денежный актив.
/// </summary>
public abstract class MonetaryAssetDto : AssetDto
{
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}
