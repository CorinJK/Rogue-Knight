using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;             // Сколько очков собрали
        public Action Changed;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}