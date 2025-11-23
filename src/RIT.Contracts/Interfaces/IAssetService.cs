using RIT.Contracts.Models;
using RIT.Contracts.Models.Requests;

namespace RIT.Contracts.Interfaces;

public interface IAssetService
{
    Task<IReadOnlyCollection<AssetDto>> GetAllAssetsAsync();
    Task<AssetDto> CreateMonetaryAssetAsync(CreateMonetaryAssetRequest request);
    Task<AssetDto> CreateNonMonetaryAssetAsync(CreateNonMonetaryAssetRequest request);
    Task<AssetDto?> UpdateMonetaryAssetAsync(int id, CreateMonetaryAssetRequest request);
    Task<AssetDto?> UpdateNonMonetaryAssetAsync(int id, CreateNonMonetaryAssetRequest request);
    Task<bool> DeleteAssetAsync(int id);
}
