namespace RIT.Contracts.Models;

/// <summary>
/// 
/// </summary>
public class RealEstateAssetDto : NonMonetaryAssetDto
{
    public required string Address { get; set; }
    public int BuildYear { get; set; }
}
