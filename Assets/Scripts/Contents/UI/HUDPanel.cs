using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{

    [Header("Coin Panel Parameters")]
    [SerializeField] Image _coinImage;
    [SerializeField] TextMeshProUGUI _currntCoinText;
    [SerializeField] TextMeshProUGUI _countText;

    // Animation parameters
    bool _isCoinUIVisible = false;
    int _countCoin;
    float _countingDuration = 0.3f;
    float _fadeDuration = 1f;
    float _timeout = 2f;
    float _maxAlpha = 0.2f;

    Coroutine _coTimeout;


    void Awake()
    {
        Color currentColor = _countText.color;
        currentColor.a = 0f;
        _countText.color = currentColor;

        _coinImage.enabled = false;
    }

    void Start()
    {
        InventoryManager.Instance.OnCoinChanged += AddCoin;
        
    }
    #region Add Coin

    void SetCoinVisible(bool visible)
    {
        if(visible)
        {
            _coinImage.enabled = true;
            _currntCoinText.DOFade(1, _fadeDuration);
        }
        else
        {
            _coinImage.enabled = false;
            _currntCoinText.DOFade(0, _fadeDuration);
        }
        
    }
    
    public void AddCoin(int amount)
    {
        if (!_isCoinUIVisible)
        {
            SetCoinVisible(true);
            _isCoinUIVisible = true;
        }
        _countCoin += amount;
        _countText.text = "+ " +_countCoin.ToString(); // update UI

        if (_countText.color.a < _maxAlpha)
        {
            _countText.DOFade(_maxAlpha, _fadeDuration);
        }
        if (_coTimeout != null)
        {
            StopCoroutine(_coTimeout);
        }
        _coTimeout = StartCoroutine(CoAddCoinTimer());
    }

    private IEnumerator CoAddCoinTimer()
    {
        yield return new WaitForSeconds(_timeout);

        // timeout -> update UI
        int startValue = Mathf.Max(InventoryManager.Instance.Coin - _countCoin, 0);
        int targetValue = InventoryManager.Instance.Coin;

        CurrentCoinCounting(startValue, targetValue);
        CountCoinCounting(_countCoin, 0);
        _countText.DOFade(0, _fadeDuration);
        _countCoin = 0;
    }
    #endregion

    #region Animation Methods
    void CurrentCoinCounting(int start, int end)
    {
        DOTween.To(() => start, x =>
        {
            start = x;
            _currntCoinText.text = x.ToString(); // update UI text
        }, end, _countingDuration).SetEase(Ease.Linear);
    }

     void CountCoinCounting(int start, int end)
    {
        DOTween.To(() => start, x =>
        {
            start = x;
            _countText.text = "+ " + x.ToString(); // update UI text
        }, end, _countingDuration).SetEase(Ease.Linear);
    }
    #endregion

}
