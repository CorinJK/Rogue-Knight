using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        // Получить внешней зависимостью CoroutineRunner
        public SceneLoader(ICoroutineRunner coroutineRunner) => 
            _coroutineRunner = coroutineRunner;

        // Запустить корутину
        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        // Загрузка сцены: ее имя и то что нужно сделать с ней
        public IEnumerator LoadScene(string nextScene, Action onLoaded)
        {
            // Чтобы не загружал сцену, которая уже активна
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();     // Запустить тех, кому нужна была инфа
                yield break;            // Закончить выполнение корутины
            }

            // Асинхронная загрузка, т.е. запросили загрузку сцены
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            // Ждем пока операция НЕ закончится
            while (!waitNextScene.isDone)
                yield return null;

            // Этот код выполняется на след кадре после yield return null
            onLoaded?.Invoke();
        }
    }
}