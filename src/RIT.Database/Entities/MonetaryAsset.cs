namespace RIT.Database.Entities;

public abstract class MonetaryAsset : Asset
{
    public decimal Amount { get; set; }
    public required string Currency { get; set; }
}
