using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour
{
    PlayerController _controller;
    PlayerMovementData _data;
    PlayerStat _stat;
    PlayerAnimation _anim;
    Rigidbody2D _rigid;

    public event Action <float> OnModifySoul;

    [Header("Heal Settings")]
    [SerializeField] Transform _startPos;
    [SerializeField] RectTransform _targetPos; // Should be the handle in Hpbar slider
    string _healEffectPath = "FX/Collection Effect";

    [Tooltip("Particle effect when the player uses a heal skill")]
    [SerializeField] ParticleSystem _energyFX;

    Coroutine _coHold;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        SubscribeEvent();
    }

    public void Init(PlayerMovementData data, PlayerStat stat, PlayerAnimation anim, Rigidbody2D rigid)
    {
        _data = data;
        _stat = stat;
        _anim = anim;
        _rigid = rigid;
    }
    void Update()
    {
        if (_controller.Input.IsHealPressed && _coHold == null)
        {
            OnHealInput();
        }
    }
    void SubscribeEvent()
    {
        _controller.Input.OnHealInputUp += OnHealUpInput;
    }

    #region Input
    void OnHealInput()
    {
        _energyFX.Play();
        CameraController.Instance.ZoomCamera(true);
        _coHold = StartCoroutine(CoHeal());
    }
    void OnHealUpInput()
    {
        StopCoroutine(_coHold);
        CameraController.Instance.ZoomCamera();
        _energyFX.Stop();
        _coHold = null;
    }
    #endregion

    #region Coroutine

    IEnumerator CoHeal()
    {
        yield return new WaitForSeconds(_data._healHoldTime);

        if (Input.GetKey(KeyCode.A) && _stat.CurrentSoul >= 30)
        {
            RefreshSoul(-30f);
            StartCoroutine(CoCreateEffect());
        }
    }
    IEnumerator CoCreateEffect()
    {
        int randCount = Random.Range(2, 6);
        int randValue = Random.Range(5, 10);

        int amount = randValue / randCount;
        float last = randValue % randCount;

        Debug.Log($"Count:{randCount}, Value:{randValue}");

        for (int i = 0; i < randCount; i++)
        {
            float randTime = Random.Range(0, 0.5f);
            yield return new WaitForSeconds(randTime);

            CollectionEffect effect = ResourceManager.Instance.Instantiate(_healEffectPath).GetComponent<CollectionEffect>();

            effect.CarryValue = (i == randCount - 1) ? amount + last : amount;
            effect.EffectStart(_startPos.position, _targetPos, 1f);
        }

        _coHold = null;
    }

    #endregion


    public void RefreshSoul(float amount)
    {
        float additaionalValue = (amount * _stat.AdditionalSoul) / 100;
        float finalValue = Mathf.Floor(additaionalValue * 10f) / 10f; // 소수점 한자리까지만 

        _stat.OnRefreshSoul(amount + finalValue);
        OnModifySoul?.Invoke(amount + finalValue);
    }
}
