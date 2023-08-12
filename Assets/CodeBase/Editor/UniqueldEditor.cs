using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniqueldEditor : UnityEditor.Editor
    {
        // Для одноразовой проверки
        private void OnEnable()
        {
            var uniqueId = (UniqueId)target;       // Получаем таргет и кастим к типу

            if (IsPrefab(uniqueId))                // Чтобы в префабе просто так не менялось
                return;

            if (string.IsNullOrEmpty(uniqueId.Id))
                Generate(uniqueId);
            else
            {
                // Найти все объекты
                UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();

                // Сели нашли повторения - перегенерируем Id
                if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id))
                    Generate(uniqueId);
            }
        }

        private bool IsPrefab(UniqueId uniqueId) =>
          uniqueId.gameObject.scene.rootCount == 0;

        private void Generate(UniqueId uniqueId)
        {
            uniqueId.Id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";      // Получить строковый Id

            // Чтобы юнити не сохраняла измения в плей моде
            if (!Application.isPlaying)
            {
                // Указать юнити что мы изменили один из ее объектов в коде и надо сохранить
                EditorUtility.SetDirty(uniqueId);
                // Чтобы юнити пересохранила измененния в сцене
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        }
    }
}