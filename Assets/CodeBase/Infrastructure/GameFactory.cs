using UnityEngine;

namespace CodeBase.Infrastructure
{
    internal class GameFactory : IGameFactory
    {
        // Константы
        private const string HeroPath = "Hero/hero";
        private const string HudPath = "Hud/Hud";

        public GameObject CreateHero(GameObject at)
        {
            return Instantiate(HeroPath, at: at.transform.position);
        }

        public void CreateHud()
        {
            Instantiate(HudPath);
        }

        // Загрузка префабов 
        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        // Перегрузка: Загрузка префабов + позиция
        private static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

    }
}