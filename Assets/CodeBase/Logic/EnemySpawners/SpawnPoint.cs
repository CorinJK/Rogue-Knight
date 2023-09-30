using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public bool Slain => _slain;        // спавнер побежден
        [SerializeField] private bool _slain;

        public MonsterTypeId MonsterTypeId;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        public string Id { get; set; }

        public void Construct(IGameFactory factory) => 
            _factory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
                Spawn();
        }

        private async void Spawn()
        {
            var monster = await _factory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;

            _slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }
    }
}