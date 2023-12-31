﻿using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string InitialPointTag = "InitialPoint";

        // Обновляет данные каждый раз, когда разработчик качается редактора
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelData = (LevelStaticData)target;

            // Нарисовать кнопочку
            if (GUILayout.Button("Collect"))
            {
                levelData.EnemySpawners = 
                    FindObjectsOfType<SpawnMarker>()
                    .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
                    .ToList();

                levelData.LevelKey = SceneManager.GetActiveScene().name;

                levelData.InitialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
            }

            // Сохранить изменения
            EditorUtility.SetDirty(target);
        }
    }
}