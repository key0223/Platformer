using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterAnimation : MonoBehaviour
{
     public Animator Anim { get; private set; }

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    public virtual void UpdateAnimation(CreatureState state)
    {
        if (state == CreatureState.Idle)
        {
            Anim.Play("IDLE");
        }
        else if (state == CreatureState.Moving)
        {
            Anim.Play("MOVE");
        }
        else if(state == CreatureState.Skill)
        {
            Anim.Play("ATTACK");
        }
        else if(state == CreatureState.Damaged)
        {
            Anim.Play("HURT");
        }
        else if (state == CreatureState.Dead)
        {
            Anim.Play("DEATH");
        }
       
    }
}
