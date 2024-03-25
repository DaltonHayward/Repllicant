using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // references
    [SerializeField] CanvasGroup loadingScreen;
    [SerializeField] GameObject loadingAccent;
    // Time values
    [SerializeField] float loadingTime = 4f;
    [SerializeField] float accentFadeTime = 2f;
    [SerializeField] float fadeOutDuration = 2f;


    // Start is called before the first frame update
    void Start()
    {
        if (loadingScreen == null) { Debug.Log("no loadingScreen reference set up"); }
        else { StartCoroutine(FadeOutCanvas(loadingScreen, fadeOutDuration)); }
    }

    IEnumerator FadeOutCanvas(CanvasGroup canvasGroup, float fadeDuration)
    {
        canvasGroup.alpha = 1;
        if (loadingAccent == null) { Debug.Log("no loadingAccent reference set up"); } 
        else { StartCoroutine(FadeInImage(loadingAccent.GetComponent<Image>(), accentFadeTime)); }
        yield return new WaitForSeconds(loadingTime); // Wait for a second
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0f, fadeDuration)); // Fade out
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            canvasGroup.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    IEnumerator FadeInImage(Image image, float fadeDuration)
    {
        float startTime = Time.time;
        Color originalColor = image.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // Target fully opaque color

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            image.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }
        image.color = targetColor; // Ensure we reach the target color exactly
    }
}
