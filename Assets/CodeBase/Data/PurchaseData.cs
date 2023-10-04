using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class PurchaseData
    {
        public List<BoughtIAP> BoughtIAPs = new List<BoughtIAP>();

        public Action Changed;

        public void AddPurchase(string id)
        {
            // Ищем в листе совпадающий id
            BoughtIAP boughtIAP = Product(id);

            if (boughtIAP != null)      // Если нашли, добавить Count
                boughtIAP.Count++;
            else                        // Если не нашли, добавить новый в список
                BoughtIAPs.Add(new BoughtIAP { IAPid = id, Count = 1 });

            Changed?.Invoke();
        }

        private BoughtIAP Product(string id) => 
            BoughtIAPs.Find(x => x.IAPid == id);
    }
}