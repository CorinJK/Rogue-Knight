using UnityEngine.Purchasing;

namespace CodeBase.Services.IAP
{
    public class ProductDescription
    {
        public string Id;                       // Id
        public Product Product;                 // Ссылка на продукт из стора
        public ProductConfig Config;            // Инфа для UI
        public int AvailablePurchasesLeft;      // Количество предметов для покупки
    }
}