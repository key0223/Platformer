using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterMovement : MonoBehaviour
{
    protected MonsterAnimation _monsterAnimation;
    protected MonsterStat _stat;

    #region State Parameters

    // Timers
    protected float _elapsedAttackTime = 0;
    [SerializeField] protected float _attackInterval = 2f;

    #endregion
    [Header("Checks")]
    [SerializeField] protected Transform _hitBoxPoint;
    [SerializeField] protected Vector2 _hitBoxSize = new Vector2(0, 0);

    [Header("Layers & Tags")]
    [SerializeField] protected LayerMask _playerLayer;

    // FX Settings
    protected string _hitFXPath = "FX/Monster HitFX";

    // Movement
    protected Coroutine _coPatrol;
    protected Coroutine _coMoving;
    protected CreatureState _state = CreatureState.Idle;
    public virtual CreatureState State
    {
        get { return _state; }
        set
        {
            if (_state == value) return;
            _state = value;
            _monsterAnimation.UpdateAnimation(_state);
            if (_coPatrol != null)
            {
                StopCoroutine(_coPatrol);
                _coPatrol = null;
            }
            if (_coMoving != null)
            {
                StopCoroutine(_coMoving);
                _coMoving = null;
            }
           
        }
    }

    protected virtual void Awake()
    {
        _monsterAnimation = GetComponent<MonsterAnimation>();
        _stat = GetComponent<MonsterStat>();
    }

    protected virtual void Update()
    {
        #region Timers
        _elapsedAttackTime += Time.deltaTime;
        #endregion

        if (_elapsedAttackTime > _attackInterval)
        {
            _elapsedAttackTime = 0;
            Attack();
        }

        UpdateMovement();
    }

    protected virtual void UpdateMovement()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                break;
            case CreatureState.Dead:
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
        if (_coPatrol == null)
            _coPatrol = StartCoroutine(CoPatrol());
    }
    protected virtual void UpdateMoving()
    {
        if (_coMoving == null)
            _coMoving = StartCoroutine(CoMoving());
    }
    protected virtual void Attack()
    {
        Collider2D hit = Physics2D.OverlapBox(_hitBoxPoint.position, _hitBoxSize, 0, _playerLayer);
        if (hit != null)
        {
            PlayerMovement player = hit.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.OnDamaged(_stat.CurrentAttack);
            }
        }
    }
    public virtual void OnDamaged(float damage, PlayerMovement player = null)
    {
        State = CreatureState.Damaged;
        StartCoroutine(CoDamaged());

        _stat.OnDamaged(damage);

        if (player != null)
        {
            player.ModifySoul(_stat.DropSoulAmount);
        }
        // FX
        GameObject fxGO = ResourceManager.Instance.Instantiate(_hitFXPath);
        fxGO.transform.position = gameObject.transform.position;
        fxGO.SetActive(true);

        //Debug.Log($"Monster Current Hp : {_stat.CurrentHp}");
        if (_stat.CurrentHp <= 0)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        State = CreatureState.Dead;
        StartCoroutine(CoDead());
    }

    #region Checks

    #endregion
    #region Coroutines
    protected virtual IEnumerator CoPatrol()
    {
        float randomWait = Random.Range(1f, 3f);
        yield return Helper.GetWait(randomWait);

        float remainingTime = Helper.GetRemainingAnimationTime(_monsterAnimation.Anim);
        yield return Helper.GetWait(remainingTime);

        State = CreatureState.Moving;
    }
    protected virtual IEnumerator CoMoving()
    {
        float remainingTime = Helper.GetRemainingAnimationTime(_monsterAnimation.Anim);
        yield return Helper.GetWait(remainingTime);
        State = CreatureState.Idle;
    }
    protected IEnumerator CoDamaged()
    {
        float clipLength = Helper.GetAnimationClipLenth(_monsterAnimation.Anim, "HURT");
        yield return Helper.GetWait(clipLength);
        State = CreatureState.Idle;
    }
    protected IEnumerator CoDead()
    {
        float remainingTime = Helper.GetRemainingAnimationTime(_monsterAnimation.Anim);
        yield return Helper.GetWait(remainingTime);

        Destroy(this.gameObject);
    }
    #endregion

    #region Editor
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_hitBoxPoint.position, _hitBoxSize);
    }
    #endregion

}
