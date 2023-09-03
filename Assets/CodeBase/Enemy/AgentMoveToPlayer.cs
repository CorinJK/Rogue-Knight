using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToPlayer : Follow
    {
        private const float MinimalDistance = 1;

        public NavMeshAgent Agent;

        private Transform _heroTransform;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Update()
        {
            // Если герой инициализирован и дистанция больше минимальной - выбрать героя как направление
            if (Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private bool Initialized() => 
            _heroTransform != null;

        private bool HeroNotReached() => 
            Vector3.Distance(Agent.transform.position, _heroTransform.position) >= MinimalDistance;
    }
}