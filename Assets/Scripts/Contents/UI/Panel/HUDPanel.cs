using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HUDPanel : MonoBehaviour
{
    PlayerController _playerController;
    PlayerMovement _playerMovement;

    #region Coin UI
    [Header("Coin Panel Parameters")]
    [SerializeField] Image _coinImage;
    [SerializeField] TextMeshProUGUI _currntCoinText;
    [SerializeField] TextMeshProUGUI _countText;

    // Animation parameters
    bool _isCoinUIVisible = false;
    float _countCoin;
    float _countingDuration = 0.3f;
    float _fadeDuration = 1f;
    float _timeout = 2f;
    float _maxAlpha = 0.2f;

    Coroutine _coTimeout;
    #endregion

    #region HP UI
    [Header("HP UI Parameters")]
    [SerializeField] Slider _hpSlider;

    [Space(5f)]
    [SerializeField] Image _soulImage;
    #endregion

    #region Shield UI
    [SerializeField] GameObject[] _shieldIcons;
    #endregion
    void Awake()
    {
        Color currentColor = _countText.color;
        currentColor.a = 0f;
        _countText.color = currentColor;
        _coinImage.enabled = false;
    }

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _playerController= FindObjectOfType<PlayerController>();
        InventoryManager.Instance.OnCoinChanged += AddCoin;
        _playerMovement.OnPlayerDamged += DecreaseHp;
        //_playerMovement.OnPlayerHealed += InceaseHp;
        _playerController.PlayerAction.OnPlayerHealed += InceaseHp;
        _playerController.PlayerAction.OnModifySoul += ModifySoul;
        //_playerMovement.OnModifySoul += ModifySoul;
        _playerMovement.OnPlayerAddShield += AddShield;
        _playerMovement.OnPlayerRemoveShield += RemoveShield;

        // Slider Settings
        _hpSlider.minValue = 0f;
        _hpSlider.maxValue = _playerMovement.Stat.MaxHp;
        _hpSlider.value = _playerMovement.Stat.CurrentHp;

        // Soul Settings
        _soulImage.fillAmount = 0f;
    }

    private void Update()
    {
        //// TEST CODE
        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //   StartCoroutine(CoCreateEffect());
        //}
    }

    #region Coin Methods

    void SetCoinVisible(bool visible)
    {
        if (visible)
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

    public void AddCoin(float amount)
    {
        if (!_isCoinUIVisible)
        {
            SetCoinVisible(true);
            _isCoinUIVisible = true;
        }
        _countCoin += amount;
        _countText.text = "+ " + _countCoin.ToString(); // update UI

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
        float startValue = Mathf.Max(InventoryManager.Instance.Coin - _countCoin, 0);
        float targetValue = InventoryManager.Instance.Coin;

        CurrentCoinCounting(startValue, targetValue);
        CountCoinCounting(_countCoin, 0);
        _countText.DOFade(0, _fadeDuration);
        _countCoin = 0;
    }

    #region Animation Methods
    void CurrentCoinCounting(float start, float end)
    {
        DOTween.To(() => start, x =>
        {
            start = x;
            _currntCoinText.text = Mathf.FloorToInt(x).ToString(); // update UI text
        }, end, _countingDuration).SetEase(Ease.Linear);
    }

    void CountCoinCounting(float start, float end)
    {
        DOTween.To(() => start, x =>
        {
            start = x;
            _countText.text = "+ " + Mathf.FloorToInt(x).ToString(); // update UI text
        }, end, _countingDuration).SetEase(Ease.Linear);
    }
    #endregion
    #endregion

    #region Hp Methods
   
    void InceaseHp(float amount)
    {
        float endValue = _playerMovement.Stat.CurrentHp + amount;
        _hpSlider.DOValue(endValue, 0.5f);
    }
    void DecreaseHp(float amount)
    {
        float endValue = _playerMovement.Stat.CurrentHp - amount;
        _hpSlider.DOValue(endValue, 0.5f);
    }
    void ModifySoul(float amount)
    {
        float targetFillAmount = Mathf.Clamp(_playerMovement.Stat.CurrentSoul, 0, _playerMovement.Stat.MaxSoul) / _playerMovement.Stat.MaxSoul;
        _soulImage.DOFillAmount(targetFillAmount, 0.3f).SetEase(Ease.Linear);
    }
    #endregion

    #region Shield Methods

    public void AddShield()
    {
        GameObject shield = GetAvailableShield();
        if (shield != null)
        {
            shield.SetActive(true);
        }

    }

    public void RemoveShield()
    {
        GameObject shield = GetUsingShield();
        if(shield != null)
        {
            shield.SetActive(false);
        }
    }
    GameObject GetAvailableShield()
    {
        for (int i = 0; i < _shieldIcons.Length; i++)
        {
            if (!_shieldIcons[i].activeSelf)
            {
                return _shieldIcons[i];
            }
        }

        return null;
    }

    GameObject GetUsingShield()
    {
        for (int i = 0; i < _shieldIcons.Length; i++)
        {
            if (_shieldIcons[i].activeSelf)
            {
                return _shieldIcons[i];
            }
        }

        return null;
    }
    #endregion
    private void OnDisable()
    {
        // stop the DotTween animation in progress
        _currntCoinText.DOKill();
        _countText.DOKill();

        // Stop Coroutine
        if (_coTimeout != null)
        {
            StopCoroutine(_coTimeout);
            _coTimeout = null;
        }

        // Alpha 값 조절
        Color currentColor = _countText.color;
        currentColor.a = 0f;
        _countText.color = currentColor;

        _countCoin = 0;
        _currntCoinText.text = InventoryManager.Instance.Coin.ToString();

    }

}
