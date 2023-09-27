using CodeBase.Infrastructure.AssetManagement;
using CodeBase.StaticData;
using CodeBase.UI.Services.Windows;
using CodeBase.StaticData.Windows;
using UnityEngine;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Ads;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;

        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService, _progressService);
        }

        public void CreateUIRoot()
        {
            _uiRoot = _assets.Instantiate(UIRootPath).transform;
        }
    }
}