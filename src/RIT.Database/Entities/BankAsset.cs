namespace RIT.Database.Entities;

public class BankAsset : MonetaryAsset
{
    public required string BankName { get; set; }
    public required string AccountNumber { get; set; }
}
