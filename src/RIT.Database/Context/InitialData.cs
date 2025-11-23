using RIT.Database.Entities;

namespace RIT.Database.Context;

public class InitialData
{
    public static List<Asset> InitAssets()
    {
        return new List<Asset>
        {
            new BankAsset { Name = "Рублевый счет", BankName = "ЕвроВорБанк", AccountNumber = "№ 5", Amount = 1000, Currency = "RUB" },
            new BankAsset { Name = "Валютный счет", BankName = "Внешторгабк", AccountNumber = "№ 3", Amount = 5, Currency = "USD" },
            new CashAsset { Name = "Наличные в кассе", Amount = 100, Currency = "RUB" },
            new CashAsset { Name = "Талоны на бензин Аспек", Amount = 3000, Currency = "RUB" },
            new RealEstateAsset
            {
                Name = "Торговое здание",
                Address = "Бассейная-6",
                BuildYear = 1970,
                InitialCost = 30000,
                ResidualCost = 5000,
                EstimatedCost = 1000000,
                InventoryNumber = "7"
            },
            new InventoryItemAsset
            {
                Name = "Гвозди",
                Quantity = 100,
                Unit = "кг",
                ProductionDate = new DateTime(2000, 1, 1),
                InitialCost = 1000,
                ResidualCost = 100,
                EstimatedCost = 2000
            }
        };
    }
}
