using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private IRandomService _random;
        private int _lootMin;
        private int _lootMax;

        // Конструктор
        public void Construct(IGameFactory factory, IRandomService random)
        {
            _factory = factory;
            _random = random;
        }
        
        private void Start()
        {
            // Подписались на событие
            EnemyDeath.Happened += SpawnLoot;
        }

        // Спавп лута
        private void SpawnLoot()
        {
            LootPiece loot = _factory.CreateLoot();         // Создаем лут в фабрике
            loot.transform.position = transform.position;    // Перемещаем его под моба

            Loot lootItem = GenerateLoot();
            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot
            {
                Value =  _random.Next(_lootMin, _lootMax)
            };
        }

        // Передать характеристики лута
        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}