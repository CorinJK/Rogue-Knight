﻿using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        // Загрузить прогресс или проинициализировать новый
        public void Enter()
        {
            LoadProgressOrInitNew();
            // Куда загружать какую сцену
            _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
        }

        public void Exit()
        {
            
        }

        // Инициализируем поле Progress
        // Сервис проверят есть сохраненные данные, если нет - новый прогресс
        private void LoadProgressOrInitNew() => 
            _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();

        // Вернем первую сцену
        private PlayerProgress NewProgress()
        {
            var progress = new PlayerProgress(initialLevel: "Main");

            progress.HeroState.MaxHP = 50;
            progress.HeroState.ResetHP();

            progress.HeroStats.Damage = 1f;
            progress.HeroStats.DamageRadius = 0.5f;

            progress.HeroState.CounterCoins = 0;

            return progress;
        }
    }
}