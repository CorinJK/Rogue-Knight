using UnityEngine;

namespace CodeBase.Infrastructure
{
    // Бутстрапер бутстрапера
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;

        private void Awake()
        {
            var bootstriper = FindObjectOfType<GameBootstrapper>();

            if(bootstriper == null)
                Instantiate(BootstrapperPrefab);
        }
    }
}