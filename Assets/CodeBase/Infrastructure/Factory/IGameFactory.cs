using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    internal interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud();
    }
}