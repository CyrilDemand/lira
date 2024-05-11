using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.0f;

    public void Start()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeImage(true));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeImage(false));
    }

    private IEnumerator FadeImage(bool fadeToBlack)
    {
        float targetAlpha = fadeToBlack ? 1.0f : 0.0f;
        float alpha = fadeImage.color.a;
        float fadeSpeed = Mathf.Abs(alpha - targetAlpha) / fadeDuration;

        while (!Mathf.Approximately(alpha, targetAlpha))
        {
            alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null;
        }
    }
}