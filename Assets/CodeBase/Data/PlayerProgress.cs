using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State HeroState;             // Состояние героя
        public WorldData WorldData;         // Положение игрока
        public Stats HeroStats;             // Статы героя
        public KillData KillData;           // Очистить спавнеры
        public PurchaseData PurchaseData;   // Количество покупок

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
            PurchaseData = new PurchaseData();
        }
    }
}