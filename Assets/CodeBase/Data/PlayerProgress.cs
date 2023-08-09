﻿using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        // Состояние героя
        public State HeroState;

        // Положение игрока
        public WorldData WorldData;

        // Статы героя
        public Stats HeroStats;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
        }
    }
}