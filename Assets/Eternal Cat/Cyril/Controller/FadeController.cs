using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Une image noire pleine écran
    public float fadeDuration = 0.5f; // Durée du fondu, réduite pour être plus rapide

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: L'image de fondu n'est pas assignée.");
        }
    }

    public void FadeToBlack()
    {
        StartCoroutine(Fade(0, 1));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Assurer que la couleur finale soit correctement appliquée
        color.a = endAlpha;
        fadeImage.color = color;
    }
}