using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        // Не можем брать через сцену, тк HUD мы создаем скриптом
        private IHealth _health;

        // Получаем и подписываемся
        public void Construct(IHealth health)
        {
            _health = health;

            _health.HealthChanged += UpdateHpBar;
        }

        // Временный вариант инициализации
        private void Start()
        {
            IHealth health = GetComponent<IHealth>();

            if (health != null)
                Construct(health);
        }

        // Обновляет значения HpBar
        private void UpdateHpBar() => 
            HpBar.SetValue(_health.Current, _health.Max);

        // Удаление с отпиской
        private void OnDestroy() => 
            _health.HealthChanged -= UpdateHpBar;
    }
}