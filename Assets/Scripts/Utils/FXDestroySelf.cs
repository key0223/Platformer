using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDestroySelf : MonoBehaviour
{
    ParticleSystem _particle;
    Coroutine _coDestroy;

    void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
        _coDestroy = StartCoroutine(CoDestroy());

    }
    IEnumerator CoDestroy()
    {
        float duration = _particle.main.duration;

        yield return Helper.GetWait(duration);
        ResourceManager.Instance.Destroy(gameObject);
    }
}
