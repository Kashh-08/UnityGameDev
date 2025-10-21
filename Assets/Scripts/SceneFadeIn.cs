using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public Image fadeImage; // Assign FadePanel image here
    public float fadeDuration = 2f; // time for fade-in effect

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration); // fade from black (1) to transparent (0)
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }
}
