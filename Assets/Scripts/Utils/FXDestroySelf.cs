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
        SceneChangeManager.Instance.OnSceneChanged += OnSceneChanged;

        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
        _coDestroy = StartCoroutine(CoDestroy());

    }
    void OnDisable()
    {
        SceneChangeManager.Instance.OnSceneChanged -= OnSceneChanged;
    }
    IEnumerator CoDestroy()
    {
        float duration = _particle.main.duration;

        yield return Helper.GetWait(duration);
        ResourceManager.Instance.Destroy(gameObject);
    }

    void OnSceneChanged()
    {
        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
        ResourceManager.Instance.Destroy(gameObject);
    }
}
