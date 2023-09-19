using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        public HeroAnimator Animator;
        private State _state;

        public event Action HealthChanged;

        // Свойства
        public float Current
        {
            get => _state.CurrentHP;
            set
            {
                // Если значение ХП поменялось, подписаться на событие
                if (_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }
            }
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
            HealthChanged?.Invoke();
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