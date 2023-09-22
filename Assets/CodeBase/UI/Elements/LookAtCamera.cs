using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start () =>
            _mainCamera = Camera.main;

        // На каждом кадре поворачивать в сторону усновной камеры
        private void Update ()
        {
            Quaternion rotation = _mainCamera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
        }
    }
}