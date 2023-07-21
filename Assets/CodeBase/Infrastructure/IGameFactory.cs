using UnityEngine;

namespace CodeBase.Infrastructure
{
    internal interface IGameFactory
    {
        GameObject CreateHero(GameObject at);
        void CreateHud();
    }
}