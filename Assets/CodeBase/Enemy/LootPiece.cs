using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        public GameObject Skull;                    // Лут объект
        public GameObject PickupFxPrefab;           // Эффект подбора
        public TextMeshPro LootText;                // Текст при подборе лута
        public GameObject PickupPopup;              // Контейнер лута
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        // Конструктор
        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }
        
        public void Initialize(Loot loot)
        {
            _loot = loot;              // Поле, которое хранит данные
        }
        
        // Когда игрок наступает на лут
        void OnTriggerEnter(Collider other) => 
            Pickup();
        
        private void Pickup()
        {
            // Чтобы сработало только 1 раз
            if (_picked)
                return;

            _picked = true;

            UpdateWorldData();

            HideSkull();            // Спрятать череп
            PlayPickupFx();         // Проиграть эффекты
            ShowText();             // Показать текст

            StartCoroutine(StartDestroyTimer()); // Запустить отсчёт до удаления
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => 
            Skull.SetActive(false);

        private void PlayPickupFx() => 
            Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            LootText.text = $"{_loot.Value}";
            PickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            
            Destroy(gameObject);
        }
    }
}