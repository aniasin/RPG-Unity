using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentCoroutineFade = null;
        

        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            float value = Mathf.Clamp01(Time.deltaTime / time);
            if (currentCoroutineFade != null) StopCoroutine(currentCoroutineFade);
            currentCoroutineFade = StartCoroutine(FadeOutCoroutine(value));
            return currentCoroutineFade;
        }
        IEnumerator FadeOutCoroutine(float value)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += value;
                yield return null;
            }
        }

        public Coroutine FadeIn(float time)
        {
            float value = Mathf.Clamp01(Time.deltaTime / time);
            if (currentCoroutineFade != null) StopCoroutine(currentCoroutineFade);
            currentCoroutineFade = StartCoroutine(FadeInCoroutine(value));
            return currentCoroutineFade;
        }
        IEnumerator FadeInCoroutine(float value)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= value;
                yield return null;
            }
        }
    }
}
