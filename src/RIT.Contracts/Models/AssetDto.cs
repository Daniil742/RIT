using System.Text.Json.Serialization;

namespace RIT.Contracts.Models;

/// <summary>
/// Актив.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(BankAssetDto), typeDiscriminator: "bank")]
[JsonDerivedType(typeof(CashAssetDto), typeDiscriminator: "cash")]
[JsonDerivedType(typeof(RealEstateAssetDto), typeDiscriminator: "real_estate")]
[JsonDerivedType(typeof(InventoryItemAssetDto), typeDiscriminator: "inventory")]
public abstract class AssetDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
