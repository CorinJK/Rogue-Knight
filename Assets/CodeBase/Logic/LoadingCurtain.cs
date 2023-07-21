using System.Collections;
using UnityEngine;

namespace CodeBase.Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        public CanvasGroup Curtain;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        // Показать канвас
        public void Show()
        {
            gameObject.SetActive(true);
            Curtain.alpha = 1f;
        }

        // Выключить канвас
        public void Hide() => 
            StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            while (Curtain.alpha > 0)
            {
                Curtain.alpha -= 0.3f;
                yield return new WaitForSeconds(0.03f);
            }

            gameObject.SetActive(false);
        }
    }
}