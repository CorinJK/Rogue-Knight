using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        private ISaveLoadService _saveLoadService;

        public BoxCollider Collider;

        private void Awake()
        {
            // Получает SaveLoadService
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        // Когда игрок входит в область - сохранить
        private void OnTriggerEnter(Collider other)
        {
            _saveLoadService.SaveProgress();

            Debug.Log("Progress Saved.");
            gameObject.SetActive(false);
        }

        // Нарисовать коробку, где лежит триггер
        private void OnDrawGizmos()
        {
            if (!Collider)
                return;

            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}