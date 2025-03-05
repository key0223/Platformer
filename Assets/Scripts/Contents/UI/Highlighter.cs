using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    RectTransform _rectTransform;

    float _moveDuration = 0.3f;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void MoveToSlot(Transform transform)
    {
        Vector2 targetOffsetMin = Vector2.zero;
        Vector2 targetOffsetMax = Vector2.zero;

        gameObject.transform.SetParent(transform, true);
        gameObject.transform.SetAsFirstSibling();
        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(_rectTransform.DOMove(transform.position, _moveDuration))
                    .Join(DOTween.To(() => _rectTransform.offsetMin, x => _rectTransform.offsetMin = x, targetOffsetMin, _moveDuration))
                    .Join(DOTween.To(() => _rectTransform.offsetMax, x => _rectTransform.offsetMax = x, targetOffsetMax, _moveDuration));

    }
}
