using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionEffect : MonoBehaviour
{
    [SerializeField] float _explosionDuration = 0.25f;
    [SerializeField] float _moveDuration = 0.5f;

    public void EffectStart(Vector2 start, Vector2 end, float range)
    {
        start = start + new Vector2(0, 1) + Random.insideUnitCircle * range;
        transform.position = start;

        Vector3 worldEndPos = Camera.main.ScreenToWorldPoint(end);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(worldEndPos, _moveDuration).SetEase(Ease.InCubic)) // 목적지로 이동
               .AppendCallback(() => Destroy(gameObject));
    }
}
