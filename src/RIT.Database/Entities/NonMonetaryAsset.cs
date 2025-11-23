namespace RIT.Database.Entities;

public abstract class NonMonetaryAsset : Asset
{
    public decimal InitialCost { get; set; }
    public decimal ResidualCost { get; set; }
    public decimal EstimatedCost { get; set; }
    public string? InventoryNumber { get; set; }
}
