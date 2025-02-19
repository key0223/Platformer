using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] float _currentExp;

    public float CurrentExp { get { return _currentExp; } set {  _currentExp = value; } }

    public void HealHp(float amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + amount, MaxHp);
    }

}
