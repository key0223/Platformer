using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerStat _stat;
    PlayerAnimation _anim;

    public event Action<float> OnPlayerHealed;
    public event Action<float> OnPlayerDamaged;

    public event Action OnPlayerAddShield;
    public event Action OnPlayerRemoveShield;

    public bool IsDead { get; private set; }

    public void Init(PlayerStat stat, PlayerAnimation anim)
    {
        _stat = stat;
        _anim = anim;
    }
    public void OnHpHeal(float amount)
    {
        _stat.OnHealHp(amount);
        OnPlayerHealed?.Invoke(amount);
    }

    public void OnDamaged(float damage)
    {
        _anim.Damaged = true;

        if(_stat.CurrentShield> 0)
        {
            OnRemoveShield();
            return;
        }

        _stat.OnDamaged(damage);
        OnPlayerDamaged?.Invoke(damage);

        if(_stat.CurrentHp <= 0)
            OnDead();
    }

    public void OnAddShield()
    {
        _stat.CurrentShield++;

        OnPlayerAddShield?.Invoke();
    }
    public void OnRemoveShield()
    {
        _stat.CurrentShield--;
        OnPlayerRemoveShield?.Invoke();
    }

    void OnDead()
    {
        IsDead = true;
        _anim.OnDead();
    }
}
