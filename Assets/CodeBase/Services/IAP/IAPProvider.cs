using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    public class IAPProvider : IStoreListener
    {
        private const string IAPConfigsPath = "IAP/products";
        private IAPService _iapService;
        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }

        public event Action Initialized;

        // Проверка, стал ли доступен контроллер и extensions
        public bool IsInitialized => _controller != null && _extensions != null;

        // Инициализируем модуль
        public void Initialize(IAPService iapService)
        {
            _iapService = iapService;

            Configs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();

            Load();     // Загрузка

            // По инструкции от юнити
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Добавить список id продуктов и тип
            foreach (ProductConfig productConfig in Configs.Values)
                builder.AddProduct(productConfig.Id, productConfig.ProductType);

            // Инициализация (IStoreListener и builder)
            UnityPurchasing.Initialize(this, builder);
        }

        // Сможем что нибудь купить
        public void StartPurchase(string productId) => 
            _controller.InitiatePurchase(productId);

        // Отправляет запрос в стор на покупку, пока покупка не закончится успешно
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            foreach (Product product in _controller.products.all)
                Products.Add(product.definition.id, product);

            Initialized?.Invoke();

            Debug.Log("UnityPurchasing initialization success");
        }

        // Если все зафейлилось
        public void OnInitializeFailed(InitializationFailureReason error) => 
            Debug.Log($"UnityPurchasing OnInitializeFailed: {error}");

        // Когда покупка удалась
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"UnityPurchasing ProcessPurchase success {purchaseEvent.purchasedProduct.definition.id}");

            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        // Если покупка не удалась
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) => 
            Debug.LogError($"Product {product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction id {product.transactionID}");

        // Из ресурсов загрузить текстовый ассет (наш json (путь)), взять оттуда текст, десериализруем типов Wrapper и берем лист конфигов
        private void Load() =>
            Configs = Resources
            .Load<TextAsset>(IAPConfigsPath)
            .text
            .ToDeserialized<ProductConfigWrapper>()
            .Configs
            .ToDictionary(x => x.Id, x => x);
    }
}