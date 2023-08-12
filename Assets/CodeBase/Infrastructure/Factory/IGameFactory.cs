using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> progressReaders { get; }
        List<ISavedProgress> progressWriters { get; }
        GameObject HeroGameObject { get; }

        // Временный эвент о создании героя
        event Action HeroCreated;     
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();

        void Cleanup();
        void Register(ISavedProgressReader savedProgress);
    }
}