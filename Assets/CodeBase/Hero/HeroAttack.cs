using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        public HeroAnimator HeroAnimator;
        public CharacterController CharacterController;     // Чтобы удобнее определять стартовую точку

        private IInputService _input;
        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            // Проверка нажата ли кнопка и не находится ли герой уже в анимании атаки
            if (_input.IsAttackButtonUp() && !HeroAnimator.IsAttacking)
                HeroAnimator.PlayAttack();
        }

        // Проверяем попадание
        public void OnAttack()
        {
            // Для каждого хита
            for (int i =  0; i < Hit(); i++)
            {
                PhysicsDebug.DrawDebug(StartPoint(), _stats.DamageRadius, 1);

                // Взять коллайдер из буфера, вытащить трансформ родителя и взять компонент здоровья
                // И нанести урон, а значение урона в статах героя
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        // Получить из прогресса статы
        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        // Смотреть по какому количеству врагов попали
        // Из какой точки будет сфера + на 1 сдвиг вреред, радиус сферы, буфер коллайдеров, маска слоя 
        private int Hit() => 
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

        // Стратовая точка для расчета позиции оружия
        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
    }
}