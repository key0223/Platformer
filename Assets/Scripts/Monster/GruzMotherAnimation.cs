using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruzMotherAnimation : NpcAnimationBase
{
    GruzMotherMovement _gruzMother;
    SpriteRenderer _renderer;

    public bool IsAnticipating {private get; set; }
    public bool StartedSlam { private get; set; }
    public bool SlamUp { private get; set; }
    public bool SlamDown {  private get; set; }
    protected override void Start()
    {
        base.Start();
        _gruzMother = GetComponent<GruzMotherMovement>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void CheckAnimationState()
    {
        if(IsAnticipating)
        {
            _anim.SetTrigger("Anticipate");
            IsAnticipating = false;
        }
        if(StartedSlam)
        {
            _anim.SetTrigger("Slam");
            StartedSlam = false;
        }
        if(SlamUp)
        {
            _anim.SetTrigger("SlamUp");
            SlamUp = false;
        }
        if(SlamDown)
        {
            _anim.SetTrigger("SlamDown");
            SlamDown = false;
        }
        _anim.SetBool("IsMoving", _gruzMother.IsMoving);
        
    }

    public void UpdateDir()
    {
        if(_gruzMother.CurrentDir == Define.Dir.Right)
        {
            _renderer.flipX = true;
        }
        else if(_gruzMother.CurrentDir == Define.Dir.Left)
        {
            _renderer.flipX = false;
        }
    }
    public void OnFirstDamage()
    {
        _anim.SetTrigger("Wake");
    }

    public void OnDead()
    {
        Debug.Log("On Dead");
        _anim.SetTrigger("Dead");
    }

    
}
