using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            float value = Mathf.Clamp01(Time.deltaTime / time);
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += value;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            float value = Mathf.Clamp01(Time.deltaTime / time);
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= value;
                yield return null;
            }
        }
    }
}
