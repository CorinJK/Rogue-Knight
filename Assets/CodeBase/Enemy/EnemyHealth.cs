﻿using CodeBase.Logic;
using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        public EnemyAnimator Animator;

        [SerializeField] private float _current;
        [SerializeField] private float _max;

        public float Current
        {
            get => _current;
            set => _current = value;
        }
        public float Max
        {
            get => _max;
            set => _max = value;
        }

        // Чтобы потом сделать ХП бары
        public event Action HealthChanged;

        // Получение урона
        public void TakeDamage(float damage)
        {
            Current -= damage;

            Animator.PlayHit();

            HealthChanged?.Invoke();
        }
    }
}