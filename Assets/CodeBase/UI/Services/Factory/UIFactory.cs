using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;

        private Transform _uiRoot;

        public UIFactory(IAssets assets)
        {
            _assets = assets;
        }

        public void CreateShop()
        {
            var config = _staticData;
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(UIRootPath).transform;
        }
    }
}