using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
    public static IEnumerator CoTyping(TextMeshProUGUI tmp, string text)
    {
        tmp.text = null;

        foreach (char c in text)
        {
            tmp.text += c;
            yield return new WaitForSeconds(0.05f);

        }
    }

    public static IEnumerator CoTextFadeIn(TextMeshProUGUI tmp)
    {
        Color color = tmp.color;
        color.a = 0f;
        tmp.color = color;

        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            tmp.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        color.a = 1f;
        tmp.color = color;
    }

    public static IEnumerator CoImageFadeIn(Image image)
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            image.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        color.a = 1f;
        image.color = color;
    }
}
