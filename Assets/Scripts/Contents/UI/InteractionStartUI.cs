using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class InteractionStartUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interactionTypeText;

    CanvasGroup _canvasGroup;

    float _fadeDuration = 0.3f;
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;

    }
    public void SetInteractionTypeText(InteractionType interactionType)
    {
        string text = "";

        switch (interactionType)
        {
            case InteractionType.None:
                break;
            case InteractionType.Shop:
                text = "ªÛ¡°";
                break;
            case InteractionType.Listen:
                text = "µË±‚";
                break;
        }

        _interactionTypeText.text = text;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0, 1f));
    }
    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0));
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        _canvasGroup.alpha = start;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / _fadeDuration);

            yield return null;
        }

        _canvasGroup.alpha = end;
    }
}
