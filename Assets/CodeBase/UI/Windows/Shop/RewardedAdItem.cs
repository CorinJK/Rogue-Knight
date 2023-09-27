using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop
{
    public class RewardedAdItem : MonoBehaviour
    {
        public Button ShowAdButton;
        public GameObject[] AdActiveObjects;
        public GameObject[] AdInactiveObjects;

        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }

        // Инициализация
        public void Initialize()
        {
            ShowAdButton.onClick.AddListener(OnShowAdClicked);       // Подписаться на кнопку

            RefreshAvailableAd();                    // Обновить состояние и проверить доступна ли реклама
        }

        // Подписаться при возможной ошибке
        public void Subscribe() => 
            _adsService.RewardedVideoReady += RefreshAvailableAd;

        // Отписаться
        public void Cleanup() => 
            _adsService.RewardedVideoReady -= RefreshAvailableAd;

        // Когда кликаем на показ рекламы
        private void OnShowAdClicked() => 
            _adsService.ShowRewardedVideo(OnVideoFinished);

        // Когда видео завершилось
        private void OnVideoFinished() => 
            _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

        private void RefreshAvailableAd()
        {
            bool videoReady = _adsService.IsRewardedVideoReady;

            // Активные, если реклама готова
            foreach (GameObject adActiveObject in AdActiveObjects)
                adActiveObject.SetActive(videoReady);

            // Неактивные
            foreach (GameObject AdInactiveObject in AdInactiveObjects)
                AdInactiveObject.SetActive(!videoReady);
        }
    }
}