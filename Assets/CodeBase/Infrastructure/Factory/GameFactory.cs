using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    internal class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }

        // Создать героя
        public GameObject CreateHero(GameObject at) => 
            _assets.Instantiate(AssetPath.HeroPath, at: at.transform.position);

        // Создать Hud
        public void CreateHud() => 
            _assets.Instantiate(AssetPath.HudPath);
    }
}