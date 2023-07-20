using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        public float RotationAngleX;
        public int Distance;
        public float OffsetY;

        [SerializeField] private Transform _following;

        private void LateUpdate()
        {
            // Если не за кем следовать, выйти
            if (_following == null) return;

            // Вращение только по оси Х
            Quaternion rotation = Quaternion.Euler(RotationAngleX, 0, 0);

            // Позиция камеры (вращение * вектор сдвига камеры)
            Vector3 position = rotation * new Vector3(0, 0, -Distance) + FollowingPointPosition();

            // Применить к камере вращение и перемещение
            transform.rotation = rotation;
            transform.position = position;
        }

        // Задать объект следования
        public void Follow(GameObject following) =>
             _following = following.transform;

        // Следование камеры за героем
        private Vector3 FollowingPointPosition()
        {
            //Берем позицию за кем следуем и добавляем OffsetY
            Vector3 followingPosition = _following.position;
            followingPosition.y += OffsetY;

            return followingPosition;
        }
    }
}