using Microsoft.EntityFrameworkCore;
using RIT.Contracts.Interfaces;
using RIT.Contracts.Models;
using RIT.Contracts.Models.Requests;
using RIT.Database.Context;
using RIT.Database.Entities;

namespace RIT.Infrastructure.Services;

internal class AssetService(
    IDbContextFactory<AssetsDbContext> contextFactory
    ) : IAssetService
{
    private readonly IDbContextFactory<AssetsDbContext> _contextFactory = contextFactory;

    public async Task<IReadOnlyCollection<AssetDto>> GetAllAssetsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entities = await context.Assets.AsNoTracking().ToArrayAsync();

        return entities.Select(MapToDto).ToArray();
    }

    public async Task<AssetDto> CreateMonetaryAssetAsync(CreateMonetaryAssetRequest model)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        Asset entity = model.Type.ToLower() switch
        {
            "bank" => new BankAsset
            {
                Name = model.Name,
                Amount = model.Amount,
                Currency = model.Currency,
                BankName = model.BankName ?? string.Empty,
                AccountNumber = model.AccountNumber ?? string.Empty
            },
            "cash" => new CashAsset
            {
                Name = model.Name,
                Amount = model.Amount,
                Currency = model.Currency
            },
            _ => throw new ArgumentException($"Неизвестный тип денежного актива: {model.Type}")
        };

        context.Assets.Add(entity);
        await context.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task<AssetDto> CreateNonMonetaryAssetAsync(CreateNonMonetaryAssetRequest model)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        Asset entity = model.Type.ToLower() switch
        {
            "real_estate" => new RealEstateAsset
            {
                Name = model.Name,
                InitialCost = model.InitialCost,
                ResidualCost = model.ResidualCost,
                EstimatedCost = model.EstimatedCost,
                InventoryNumber = model.InventoryNumber,
                Address = model.Address ?? string.Empty,
                BuildYear = model.BuildYear ?? 0
            },
            "inventory" => new InventoryItemAsset
            {
                Name = model.Name,
                InitialCost = model.InitialCost,
                ResidualCost = model.ResidualCost,
                EstimatedCost = model.EstimatedCost,
                InventoryNumber = model.InventoryNumber,
                Quantity = model.Quantity,
                Unit = model.Unit,
                ProductionDate = model.ProductionDate.Value
            },
            _ => throw new ArgumentException($"Неизвестный тип имущества: {model.Type}")
        };

        context.Assets.Add(entity);
        await context.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task<AssetDto?> UpdateMonetaryAssetAsync(int id, CreateMonetaryAssetRequest model)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Assets.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            return null;

        entity.Name = model.Name;

        if (entity is BankAsset bankAccount)
        {
            bankAccount.Amount = model.Amount;
            bankAccount.Currency = model.Currency;
            bankAccount.BankName = model.BankName;
            bankAccount.AccountNumber = model.AccountNumber;
        }
        else if (entity is CashAsset cash)
        {
            cash.Amount = model.Amount;
            cash.Currency = model.Currency;
        }

        await context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<AssetDto?> UpdateNonMonetaryAssetAsync(int id, CreateNonMonetaryAssetRequest model)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Assets.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            return null;

        entity.Name = model.Name;

        if (entity is NonMonetaryAsset tangible)
        {
            tangible.InitialCost = model.InitialCost;
            tangible.ResidualCost = model.ResidualCost;
            tangible.EstimatedCost = model.EstimatedCost;
            tangible.InventoryNumber = model.InventoryNumber;
        }

        if (entity is RealEstateAsset realEstate)
        {
            realEstate.Address = model.Address;
            realEstate.BuildYear = model.BuildYear ?? 0;
        }
        else if (entity is InventoryItemAsset inventory)
        {
            inventory.Quantity = model.Quantity;
            inventory.Unit = model.Unit;
            inventory.ProductionDate = model.ProductionDate.Value;
        }

        await context.SaveChangesAsync();
        return MapToDto(entity);
    }

    public async Task<bool> DeleteAssetAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var asset = await context.Assets.FindAsync(id);

        if (asset == null)
            return false;

        context.Assets.Remove(asset);

        return await context.SaveChangesAsync() > 0;
    }

    private AssetDto MapToDto(Asset entity)
    {
        return entity switch
        {
            BankAsset m => new BankAssetDto
            {
                Id = m.Id,
                Name = m.Name,
                Amount = m.Amount,
                Currency = m.Currency,
                BankInfo = string.IsNullOrEmpty(m.BankName)
                    ? "Наличные / Касса"
                    : $"{m.BankName} (Счет: {m.AccountNumber})"
            },
            CashAsset c => new CashAssetDto
            {
                Id = c.Id,
                Name = c.Name,
                Amount = c.Amount,
                Currency = c.Currency,
            },
            RealEstateAsset r => new RealEstateAssetDto
            {
                Id = r.Id,
                Name = r.Name,
                InitialCost = r.InitialCost,
                ResidualCost = r.ResidualCost,
                EstimatedCost = r.EstimatedCost,
                InventoryNumber = r.InventoryNumber,
                Address = r.Address,
                BuildYear = r.BuildYear,
            },
            InventoryItemAsset t => new InventoryItemAssetDto
            {
                Id = t.Id,
                Name = t.Name,
                InitialCost = t.InitialCost,
                ResidualCost = t.ResidualCost,
                EstimatedCost = t.EstimatedCost,
                InventoryNumber = t.InventoryNumber,
                Quantity = t.Quantity,
                Unit = t.Unit,
                ProductionDate = t.ProductionDate,
            },
            _ => throw new NotImplementedException()
        };
    }
}
