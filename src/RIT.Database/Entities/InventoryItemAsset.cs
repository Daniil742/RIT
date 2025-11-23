namespace RIT.Database.Entities;

public class InventoryItemAsset : NonMonetaryAsset
{
    public double Quantity { get; set; }
    public string Unit { get; set; } = "шт";
    public DateTime ProductionDate { get; set; }
}
