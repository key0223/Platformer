using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_ElderbugAnimation : NpcAnimationBase
{
    Npc_ElderbugController _npc;

    protected override void Start()
    {
        base.Start();
        _npc = GetComponent<Npc_ElderbugController>();
    }

    protected override void CheckAnimationState()
    {
        _anim.SetBool("IsIdle", _npc.IsIdle);
        _anim.SetBool("IsCallingPlayer", _npc.IsPassingBy);
        _anim.SetBool("IsPlayerOnTheRight", _npc.IsPlayerOnTheRight);
        _anim.SetBool("IsTalking", _npc.IsTalking);
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
