using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth Health;

        public HeroMove Move;
        public HeroAnimator Animator;

        public GameObject DeathFx;
        private bool _isDead;

        // Подписаться на событие
        private void Start() =>
            Health.HealthChanged += HealthChanged;

        // Отписаться от события
        private void OnDestroy() =>
            Health.HealthChanged -= HealthChanged;

        // Меняется ХП героя
        private void HealthChanged()
        {
            // Не выполнились ли условия для смерти
            if (!_isDead && Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            _isDead = true;             // Чтобы не было много смертей подряд

            Move.enabled = false;       // Отключить движение
            Animator.PlayDeath();       // Проиграть смерть

            // Что инстанциировать, где, и то что ничего менять не надо по направдению
            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }
    }
}