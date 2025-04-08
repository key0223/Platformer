using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_IseldaAnimation : NpcAnimationBase
{
    Npc_IseldaController _npc;

    public bool StartedIdle { private get; set; }
    public bool StartedTalking { private get; set; }
    public bool StartedTalkingRight {  private get; set; }
    protected override void Start()
    {
        base.Start();
        _npc = GetComponent<Npc_IseldaController>();
    }

    protected override void CheckAnimationState()
    {
        if(StartedIdle)
        {
            _anim.SetTrigger("Idle");
            StartedIdle = false;
        }
        _anim.SetBool("IsTalking", _npc.IsTalking);
        _anim.SetBool("IsPlayerOnTheRight", _npc.IsPlayerOnTheRight);
    }

    public void UpdateDirection()
    {
        if (_npc.PlayerDir == Dir.Right)
        {
            _anim.ResetTrigger("TurnLeft");
            _anim.SetTrigger("TurnRight");
        }
        else
        {
            _anim.ResetTrigger("TurnRight");
            _anim.SetTrigger("TurnLeft");
        }
    }
}
