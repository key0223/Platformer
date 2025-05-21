using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour
{
    PlayerController _controller;
    PlayerMovementData _data;
    PlayerStat _stat;
    PlayerAnimation _anim;

    public event Action <float> OnModifySoul;

    [Header("Heal Settings")]
    [SerializeField] Transform _startPos;
    [SerializeField] RectTransform _targetPos; // Should be the handle in Hpbar slider
    string _healEffectPath = "FX/Collection Effect";

    [Tooltip("Particle effect when the player uses a heal skill")]
    [SerializeField] ParticleSystem _energyFX;

    [Space(5f)]
    [Header("Attack Settings")]
    [SerializeField] Transform _frontAttackCheckPoint;
    [SerializeField] Vector2 _attackCheckSize = new Vector2(1f, 1f);
    
    int _attackableLayer = (1 << (int)Layer.Monster) | (1 << (int)Layer.Breakable);
    public bool IsAttacking { get; private set; }

    InputBuffer _attackBuffer;

    Coroutine _coHold;

    void Awake()
    {
        SubscribeEvent();
        _attackBuffer = new InputBuffer(_data._attackInputBufferTime);
    }

    public void Init(PlayerController controller, PlayerMovementData data, PlayerStat stat, PlayerAnimation anim)
    {
        _controller = controller;
        _data = data;
        _stat = stat;
        _anim = anim;
    }
    void Update()
    {
        if(_controller.PlayerHealth.IsDead) return;

        _attackBuffer.Update(Time.deltaTime);
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

    public void OnAttackInput()
    {
        _anim.StartedAttacking = true;
        _attackBuffer.Set();
    }
    #endregion

    /* Coroutine */

    #region Heal Coroutine
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

    #region Attack Coroutine
    public IEnumerator CoAttack(Action<float> sleep)
    {
        IsAttacking = true;
        Collider2D[] hit;
        hit = Physics2D.OverlapBoxAll(_frontAttackCheckPoint.position, _attackCheckSize, 0, _attackableLayer);


        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                MonsterMovement monster = hit[i].GetComponent<MonsterMovement>();
                if (monster != null)
                {
                    monster.OnDamaged(_stat.TotalAttack, _controller);
                }

                IBreakable breakable = hit[i].GetComponent<IBreakable>();
                if (breakable != null)
                {
                    breakable.OnDamaged(1);
                }
            }
            
            CameraController.Instance.ShakeCamera();
        }

        sleep?.Invoke(0.2f);

        float remainingTime = Helper.GetRemainingAnimationTime(_anim.Anim);
        yield return new WaitForSeconds(remainingTime);

        IsAttacking = false;
        _attackBuffer.Reset();
    }
    #endregion


    /* Check */
    public bool CanAttack(bool isDashing)
    {
        if (_attackBuffer.IsActive && !isDashing && !IsAttacking)
            return true;
        else
            return false;
    }

    public void RefreshSoul(float amount)
    {
        float additaionalValue = (amount * _stat.AdditionalSoul) / 100;
        float finalValue = Mathf.Floor(additaionalValue * 10f) / 10f; // 소수점 한자리까지만 

        _stat.OnRefreshSoul(amount + finalValue);
        OnModifySoul?.Invoke(amount + finalValue);
    }

    #region Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_frontAttackCheckPoint.position, _attackCheckSize);
    }
    #endregion
}
