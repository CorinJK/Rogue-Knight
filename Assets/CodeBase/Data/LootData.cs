using CodeBase.Infrastructure.Services.PersistentProgress;
using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData : ISavedProgress
    {
        public int Collected;             // Сколько очков собрали
        public Action Changed;
        private State _state;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }

        public float Counter
        {
            get => _state.CounterCoins;
            set
            {
                // Если значение поменялось, подписаться на событие
                if (_state.CounterCoins != value)
                {
                    _state.CounterCoins = value;
                    Changed?.Invoke();
                }
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            Changed?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CounterCoins = Counter;
        }

        public void Add(int loot)
        {
            Collected += loot;
            Changed?.Invoke();
        }
    }
}