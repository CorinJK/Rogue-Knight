using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> progressReaders { get; }
        List<ISavedProgress> progressWriters { get; }

        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<LootPiece> CreateLoot();

        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
        Task WarmUp();
    }
}