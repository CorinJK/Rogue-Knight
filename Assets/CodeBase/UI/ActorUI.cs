using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.UI
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        // Не можем брать через сцену, тк HUD мы создаем скриптом
        private HeroHealth _heroHealth;

        // Удаление с отпиской
        private void OnDestroy() => 
            _heroHealth.HealthChanged -= UpdateHpBar;

        // Получаем и подписываемся
        public void Constract(HeroHealth health)
        {
            _heroHealth = health;

            _heroHealth.HealthChanged += UpdateHpBar;
        }

        // Обновляет значения HpBar
        private void UpdateHpBar()
        {
            HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
        }
    }
}