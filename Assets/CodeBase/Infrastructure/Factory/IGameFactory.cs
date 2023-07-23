using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud();
        List<ISavedProgressReader> progressReaders { get; }
        List<ISavedProgress> progressWriters { get; }
        void Cleanup();
    }
}