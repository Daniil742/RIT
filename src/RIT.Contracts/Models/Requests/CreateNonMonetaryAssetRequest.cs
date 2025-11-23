namespace RIT.Contracts.Models.Requests;

public class CreateNonMonetaryAssetRequest
{
    public required string Type { get; set; }
    public required string Name { get; set; }
    public decimal InitialCost { get; set; }
    public decimal ResidualCost { get; set; }
    public decimal EstimatedCost { get; set; }
    public string? InventoryNumber { get; set; }
    public double Quantity { get; set; } = 1;
    public string Unit { get; set; } = "шт";
    public DateTime? ProductionDate { get; set; }
    public string? Address { get; set; }
    public int? BuildYear { get; set; }
}
