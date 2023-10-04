using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Services.Ads
{
    public class AdsService : IUnityAdsListener, IAdsService
    {
        private const string AndroidGameId = "5429561";
        private const string IOSGameId = "5429560";

        private const string RewardedVideoPlacementId = "rewardedVideo";

        private string _gameId;

        private Action _onVideoFinished;
        public event Action RewardedVideoReady;

        public int Reward => 10;

        // Инициализируем
        public void Initialize()
        {
            // Зависимость от платформы
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    break;
                default:
                    Debug.Log("Платформа не поддерживается");
                    break;
            }

            Advertisement.AddListener(this);
            Advertisement.Initialize(_gameId);
        }

        // Метод, запрашивающий вызов рекламы
        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId);
            _onVideoFinished = onVideoFinished;
        }

        // Проверить, готова ли реклама к показу
        public bool IsRewardedVideoReady =>
            Advertisement.IsReady(RewardedVideoPlacementId);

        // Реклама готова к показу
        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            // Сообщить всем
            if (placementId == RewardedVideoPlacementId)
                RewardedVideoReady?.Invoke();
        }

        // Если произошла ошибка
        public void OnUnityAdsDidError(string message) =>
            Debug.Log($"OnUnityAdsDidError {message}");

        // Реклама началась
        public void OnUnityAdsDidStart(string placementId) =>
            Debug.Log($"OnUnityAdsDidStart {placementId}");

        // Окончание рекламы
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
            }

            // Обнуляем, чтобы заново запрашивали показ рекламы
            _onVideoFinished = null;
        }
    }
}