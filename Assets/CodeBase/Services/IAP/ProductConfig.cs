using System;
using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    [Serializable]
    public class ProductConfig
    {
        public string Id;                   // Id товара
        public ProductType ProductType;     // Тип товара

        public int MaxPurchaseCount;        // Ограничение на покупку
        public ItemType ItemType;           // Предмет из списка
        public int Quantity;                // Количество
    }
} 