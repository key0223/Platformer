using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerController _controller;
    PlayerStat _stat;
    PlayerAnimation _anim;

    public event Action<float> OnPlayerHealed;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _stat = _controller.PlayerStat;
        _anim = _controller.Anim;
    }

    public void OnHpHeal(float amount)
    {
        _stat.OnHealHp(amount);
        OnPlayerHealed?.Invoke(amount);
    }
}
