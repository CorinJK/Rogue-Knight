using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        public float Speed;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;
        private Vector3 _positionToLook;

        // Некрасиво пока что получаем фабрику для инициализации героя
        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            // Если герой уже существует у фабрики
            if (HeroExists())
                InitializeHeroTransform();
            // Подписываемся на ивент создания героя
            else
                _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            // Если герой инициализирован и дистанция больше минимальной - выбрать героя как направление
            if (Initialized())
                RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLook();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        // Ниже все математические расчеты для плавного поворота
        private void UpdatePositionToLook()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) => 
            Quaternion.Lerp(rotation, TargerRotation(positionToLook), SpeedFactory());

        private Quaternion TargerRotation(Vector3 position) => 
            Quaternion.LookRotation(position);

        private float SpeedFactory() =>
            Speed * Time.deltaTime;

        private bool HeroExists() =>
            _gameFactory.HeroGameObject != null;

        // Просто берем героя у фабрики
        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool Initialized() =>
            _heroTransform != null;

    }
}