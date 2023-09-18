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

        GameObject CreateHero(GameObject at);
        GameObject CreateHud();

        void Cleanup();
        void Register(ISavedProgressReader savedProgress);
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
    }
}