using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_ElderbugAnimation : NpcAnimationBase
{
    Npc_ElderbugController _npc;

    public bool StartedIdle { private get; set; }
    protected override void Start()
    {
        base.Start();
        _npc = GetComponent<Npc_ElderbugController>();
    }

    protected override void CheckAnimationState()
    {
        if(StartedIdle)
        {
            _anim.SetTrigger("Idle");
            StartedIdle = false;
        }


        _anim.SetBool("IsCallingPlayer", _npc.IsPassingBy);
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
