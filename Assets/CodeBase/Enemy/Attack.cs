using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        public EnemyAnimator Animator;

        public float AttackColldown = 3f;
        public float CLeavage = 0.5f;               // Радиус сферы
        public float EffectiveDistance = 0.5f;
        public float Damage = 10f;

        private float _attackColldown;              // Текущая инфа о CD
        private bool _isAttacking;

        private Transform _heroTransform;           // Чтобы поворачиваться к герою во время атаки
        private int _layerMask;
        private Collider[] _hits = new Collider[1]; // Буфер коллайдеров
        private bool _attackIsActive;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer("Player");              // Определение слоя маски
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
            // Проверяем что hit произошел
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), CLeavage, 1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private bool Hit(out Collider hit)
        {
            // Нам нужна позиция, размер сферы, буфер и маска со слоями
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), CLeavage, _hits, _layerMask);

            // Достать из буфера, если есть
            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

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
            _attackIsActive && !_isAttacking && CooldownIsUp();

        private bool CooldownIsUp() => 
            _attackColldown <= 0;
    }
}