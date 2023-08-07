using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        public HeroAnimator Animator;
        private State _state;

        // Свойства
        public float Current
        {
            get => _state.CurrentHP;
            set => _state.CurrentHP = value;
        }
        public float Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        // Взять прогресс
        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
        }

        // Сохранить в прогресс
        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }

        // Метод нанесения урона
        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            Current -= damage;
            Animator.PlayHit();
        }
    }
}