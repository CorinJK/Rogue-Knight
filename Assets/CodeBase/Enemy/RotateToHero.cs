using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        public float Speed;

        private Transform _heroTransform;
        private Vector3 _positionToLook;

        public void Construct(Transform heroTransform) =>
            _heroTransform = heroTransform;

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

        private bool Initialized() =>
            _heroTransform != null;

    }
}