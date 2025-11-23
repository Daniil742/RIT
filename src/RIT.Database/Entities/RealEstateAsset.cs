namespace RIT.Database.Entities;

public class RealEstateAsset : NonMonetaryAsset
{
    public required string Address { get; set; }
    public int BuildYear { get; set; }
}
