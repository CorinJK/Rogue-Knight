using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;

        public float Cooldown;

        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;       // Чтобы юнити не багался

        // Подписаться на ивент
        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            // Выключить скрипт AgentMoveToPlayer
            SwitchFollowOff();
        }

        // Остановить корутину отсчета и включить преследование
        private void TriggerEnter(Collider obj)
        {
            if (!_hasAggroTarget)
            {
                _hasAggroTarget = true;
                StopAggroCoroutine();

                SwitchFollowOn();
            }
        }

        private void TriggerExit(Collider obj)
        {
            if (_hasAggroTarget)
            {
                _hasAggroTarget = false;

                // Запуск корутины
                _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
            }
        }

        // Ждем со след кадра неск секунд и отключаем преследование
        private IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return new WaitForSeconds(Cooldown);
            SwitchFollowOff();
        }

        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
            {
                StopCoroutine(_aggroCoroutine);
                _aggroCoroutine = null;
            }
        }

        // Включить скрипт преследования
        private void SwitchFollowOn() =>
            Follow.enabled = true;

        // Выключить скрипт преследования
        private void SwitchFollowOff() =>
            Follow.enabled = false;
    }
}