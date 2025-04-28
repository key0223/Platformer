using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class InteractionStartUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interactionTypeText;

    RectTransform _rectTrasnform;

    CanvasGroup _canvasGroup;

    float _fadeDuration = 0.3f;
    void Start()
    {
        _rectTrasnform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;

    }
    public void SetUI(InteractionType interactionType, GameObject target)
    {
        _rectTrasnform.position = new Vector3(target.transform.position.x, target.transform.position.y+1);
        string text = "";

        switch (interactionType)
        {
            case InteractionType.None:
                break;
            case InteractionType.Shop:
                text = "상점";
                break;
            case InteractionType.Listen:
                text = "듣기";
                break;
            case InteractionType.Examine:
                text = "조사";
                break;
            case InteractionType.Door:
                text = "들어가기";
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
