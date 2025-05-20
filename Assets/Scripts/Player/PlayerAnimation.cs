using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerMovement _playerMove;
    Animator _anim;
    SpriteRenderer _renderer;
    public Animator Anim { get { return _anim; } }
    
    public bool StartedJumping { private get; set; }
    public bool JustLanded { private get; set; }
    public bool StartedAttacking {  private get; set; }
    public bool Damaged { private get; set; }

    protected bool _isDead;

    // 피격 효과
    WaitForSeconds _delay = new WaitForSeconds(0.1f);
    Coroutine _coFlicker;
    int _repeat = 4;

    void Start()
    {
        _playerMove = GetComponent<PlayerMovement>();
        _anim = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if(_isDead) return;
        CheckAnimationState();
    }
    void CheckAnimationState()
    {
        if(StartedJumping)
        {
            _anim.SetTrigger("Jump");
            StartedJumping = false;
            return;
        }

        if(JustLanded)
        {
            _anim.SetTrigger("Land");
            JustLanded = false;
            return;
        }
       
        if(StartedAttacking) 
        {
            _anim.SetTrigger("Attack");
            StartedAttacking = false;
            return;
        }

        if(Damaged)
        {
            if (_coFlicker == null)
            {
                _coFlicker = StartCoroutine(CoFlicker());
            }
        }
      
        _anim.SetBool("IsWallSliding", _playerMove.IsSliding);
        _anim.SetBool("IsGroundSliding", _playerMove.IsDashing);
        _anim.SetBool("IsAttacking", _playerMove.IsAttacking);
        _anim.SetFloat("Vel Y", _playerMove.RB.velocity.y);
        _anim.SetFloat("Vel X", Mathf.Abs(_playerMove.RB.velocity.x));

    }

    #region Dead
    public void OnDead()
    {
        _isDead = true;
        _anim.SetBool("IsDead", _playerMove.IsDead);
    }
    #endregion
    IEnumerator CoFlicker()
    {
        for (int i = 0; i < _repeat; i++)
        {
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 0.5f);
            yield return _delay;

            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 1f);
            yield return _delay;
        }
        Damaged = false;
        _coFlicker = null;
    }

}
