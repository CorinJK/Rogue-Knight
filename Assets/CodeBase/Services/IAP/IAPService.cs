using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    public class IAPService
    {
        private readonly IAPProvider _iapProvider;
        private readonly IPersistentProgressService _progressService;

        public bool IsInitialized => _iapProvider.IsInitialized;
        public event Action Initialized;

        public IAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            _iapProvider = iapProvider;
            _progressService = progressService;
        }

        // Инициализируем и подписываемся
        public void Initialize()
        {
            _iapProvider.Initialize(this);
            _iapProvider.Initialized += () => Initialized.Invoke();
        }

        // Будем отдавать наружу
        public List<ProductDescription> Products() =>
            ProductsDescription().ToList();

        public void StartPurchase(string productId) => 
            _iapProvider.StartPurchase(productId);

        public PurchaseProcessingResult ProcessPurchase(Product purchasedProduct)
        {
            ProductConfig productConfig = _iapProvider.Configs[purchasedProduct.definition.id];

            switch (productConfig.ItemType)
            {
                case ItemType.Skulls:
                    _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);           // Присвоить очки
                    _progressService.Progress.PurchaseData.AddPurchase(purchasedProduct.definition.id); // Учесть в количество
                    break;
            }

            return PurchaseProcessingResult.Complete;
        }

        private IEnumerable<ProductDescription> ProductsDescription()
        {
            PurchaseData purchaseData = _progressService.Progress.PurchaseData;         // Что уже купили

            // Для каждого продукта
            foreach (string productId in _iapProvider.Products.Keys)
            {
                ProductConfig config = _iapProvider.Configs[productId];     // Взяли конфиг
                Product product = _iapProvider.Products[productId];         // Взяли продукт

                // Есть ли продукт, совпадающий с текущем рассматриваемым
                BoughtIAP boughtIAP = purchaseData.BoughtIAPs.Find(x => x.IAPid == productId);

                // Проверить есть ли купленный и не превысило ли значение покупок
                if (ProductBoughtOut(config, boughtIAP))
                    continue;

                // Если предмет доступен для покупок
                yield return new ProductDescription
                {
                    Id = productId,
                    Config = config,
                    Product = product,
                    AvailablePurchasesLeft = boughtIAP != null          // Если покупки не было
                    ? config.MaxPurchaseCount - boughtIAP.Count         // Вычесть
                    : config.MaxPurchaseCount,                          // Указать максимальное количество
                };
            }
        }

        private static bool ProductBoughtOut(ProductConfig config, BoughtIAP boughtIAP) => 
            boughtIAP != null && boughtIAP.Count >= config.MaxPurchaseCount;
    }
}