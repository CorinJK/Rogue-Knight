using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackColldown = 3f;

        private float _attackColldown;      // Текущая инфа о CD
        private bool _isAttacking;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;           // Чтобы поворачиваться к герою во время атаки

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _gameFactory.HeroCreated += OnHeroCreated;                      // Подписались на ивент
        }

        private void Update()
        {
            // Если больше 0, то постепенно уменьшать СD
            UpdateCooldown();

            // Если меньше 0, произвести атаку
            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
        }

        private void OnAttackEnded()
        {
            // Будет хранить текущую инфу о CD
            _attackColldown = AttackColldown;

            _isAttacking = false;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _attackColldown -= Time.deltaTime;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);   // Посмотреть на героя
            Animator.PlayAttack();              // Проиграть атаку

            _isAttacking = true;
        }

        private bool CanAttack() => 
            !_isAttacking && CooldownIsUp();

        private bool CooldownIsUp() => 
            _attackColldown <= 0;

        // Получили hero трансформ
        private void OnHeroCreated() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;
    }
}