namespace RIT.Contracts.Models;

/// <summary>
/// 
/// </summary>
public class InventoryItemAssetDto : NonMonetaryAssetDto
{
    public double Quantity { get; set; }
    public string Unit { get; set; } = "шт";
    public DateTime ProductionDate { get; set; }
}
