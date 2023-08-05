using UnityEngine;

namespace CodeBase.Logic
{
    public class AnimatorStateReporter : StateMachineBehaviour
    {
        private IAnimationStateReader _stateReader;

        // Принимает от StateMachineBehaviour
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            FindReader(animator);

            _stateReader.EnteredState(stateInfo.shortNameHash);
        }

        // Принимает от StateMachineBehaviour
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            FindReader(animator);

            _stateReader.ExitedState(stateInfo.shortNameHash);
        }

        private void FindReader(Animator animator)
        {
            if (_stateReader != null)
                return;

            // Ищет EnemyAnimator 
            _stateReader = animator.gameObject.GetComponent<IAnimationStateReader>();
        }
    }
}