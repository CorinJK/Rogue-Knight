﻿using CodeBase.Data;

namespace CodeBase.Services.PersistentProgress
{
    // Может только читать прогресс
    public interface ISavedProgressReader
    {
        // Загрузить прогресс
        void LoadProgress(PlayerProgress progress);
    }
}