using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        // Закрыть окно
        public Button CloseButton;

        private void Awake()
        {
            OnAwake();
        }

        // Чтобы можно было переопределять
        protected virtual void OnAwake()
        {
            CloseButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}
