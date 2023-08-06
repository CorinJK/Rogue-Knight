using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero : Follow
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
            if (HeroExists())
                InitializeHeroTransform();
            // Подписываемся на ивент создания героя
            else
                _gameFactory.HeroCreated += InitializeHeroTransform;
        }

        private void Update()
        {
            // Если герой инициализирован и дистанция больше минимальной - выбрать героя как направление
            if (Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private bool HeroExists() => 
            _gameFactory.HeroGameObject != null;

        // Просто берем героя у фабрики
        private void InitializeHeroTransform() => 
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool Initialized() => 
            _heroTransform != null;

        private bool HeroNotReached() => 
            Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
    }
}