using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 1;

        public NavMeshAgent Agent;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        // Некрасиво пока что получаем фабрику для инициализации героя
        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            // Если герой уже существует у фабрики
            if (_gameFactory.HeroGameObject != null)
                InitializeHeroTransform();
            // Подписываемся на ивент создания героя
            else
                _gameFactory.HeroCreated += HeroCreated;
        }

        private void Update()
        {
            // Если герой инициализирован и дистанция больше минимальной - выбрать героя как направление
            if (Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        // Просто берем героя у фабрики
        private void InitializeHeroTransform() => 
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool Initialized() => 
            _heroTransform != null;

        private void HeroCreated() => 
            InitializeHeroTransform();

        private bool HeroNotReached() => 
            Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
    }
}