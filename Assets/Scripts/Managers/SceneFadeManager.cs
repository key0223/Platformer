using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : SingletonMonobehaviour<SceneFadeManager>
{
    [SerializeField] Image _fadeOutImage;
    [Range(0.1f,10f)]
    [SerializeField] float _fadeOutSpeed = 5f; 
    [Range(0.1f,10f)]
    [SerializeField] float _fadeInSpeed = 5f;

    [SerializeField] Color _fadeOutStartColor;

    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        _fadeOutStartColor.a = 0f;
    }

    void Update()
    {
        if(IsFadingOut)
        {
            if(_fadeOutImage.color.a <1f)
            {
                _fadeOutStartColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;
            }
        }

        if(IsFadingIn)
        {
            if (_fadeOutImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        _fadeOutImage.color = _fadeOutStartColor;
        IsFadingOut = true;
    }
    public void StartFadeIn()
    {

        if(_fadeOutImage.color.a >= 1f)
        {
            _fadeOutImage.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }
}
