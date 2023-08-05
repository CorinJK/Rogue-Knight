using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyAnimator))]
    public class AnimateAlongAgent : MonoBehaviour
    {
        private const float MinimalVeloсity = 0.1f;

        public NavMeshAgent Agent;
        public EnemyAnimator Animator;

        public void Update()
        {
            // Если у агента есть какая то скорость - перевести аниматор в движение
            // magnitude - длина вектора
            if (ShouldMove())
                Animator.Move(Agent.velocity.magnitude);
            // Остановить анимацию
            else
                Animator.StopMoving();
        }

        // Если скорость больше минимальной и остаточная дистанция больше радиуса
        private bool ShouldMove() =>
            Agent.velocity.magnitude > MinimalVeloсity && Agent.remainingDistance > Agent.radius;
    }
}