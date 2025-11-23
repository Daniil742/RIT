namespace RIT.Contracts.Models;

/// <summary>
/// Неденежный актив.
/// </summary>
public abstract class NonMonetaryAssetDto : AssetDto
{
    public decimal InitialCost { get; set; }
    public decimal ResidualCost { get; set; }
    public decimal EstimatedCost { get; set; }
    public string? InventoryNumber { get; set; }
}
