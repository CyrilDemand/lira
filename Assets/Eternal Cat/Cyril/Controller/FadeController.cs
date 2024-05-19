using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // Une image noire pleine écran
    public float fadeDuration = 0.5f; // Durée du fondu, réduite pour être plus rapide
    public float slideDuration = 0.5f; // Durée du slide

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

    public void InstantBlack()
    {
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;
    }

    public IEnumerator SlideBlackLeftToRight(GameObject player)
    {
        yield return StartCoroutine(SlideBlackLeftToRightCoroutine());
 
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

    public IEnumerator SlideBlackLeftToRightCoroutine()
    {
        float elapsedTime = 0f;
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();

        Vector2 startPos = new Vector2(-Screen.width, 0); // Hors écran à gauche
        Vector2 endPos = new Vector2(Screen.width, 0); // Hors écran à droite

        rectTransform.anchoredPosition = startPos;

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / slideDuration);
            yield return null;
        }

        rectTransform.anchoredPosition = endPos; // Assurer que la position finale soit correctement appliquée
    }
    
    

}
