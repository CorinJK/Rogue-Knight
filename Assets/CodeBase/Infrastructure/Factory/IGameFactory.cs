using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> progressReaders { get; }
        List<ISavedProgress> progressWriters { get; }

        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        LootPiece CreateLoot();

        void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
    }
}