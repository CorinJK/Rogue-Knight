using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;

        protected IPersistentProgressService ProgressService;
        protected PlayerProgress Progress => ProgressService.Progress;

        public void Construct(IPersistentProgressService progressService) => 
            ProgressService = progressService;

        private void Awake() => 
            OnAwake();

        private void Start()
        {
            Initialize();
            SubsсribeUpdates();
        }

        private void OnDestroy() => 
            Cleanup();

        // Чтобы можно было переопределять
        protected virtual void OnAwake() => 
            CloseButton.onClick.AddListener(() => Destroy(gameObject));

        // Инициализация данных
        protected virtual void Initialize()
        {
        }

        // Узнавать об изменении данных
        protected virtual void SubsсribeUpdates()
        {
        }

        // Чтобы отписаться
        protected virtual void Cleanup()
        {
        }
    }
}
