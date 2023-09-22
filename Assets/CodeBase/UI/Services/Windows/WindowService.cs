using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        // Открыть окно меню
        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknow:
                    break;
                case WindowId.Shop:
                    _uiFactory.CreateShop();
                    break;
            }
        }
    }
}
