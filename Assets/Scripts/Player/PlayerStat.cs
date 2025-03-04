using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class PlayerStat : Stat
{
    [SerializeField] float _currentExp;
    [SerializeField] float _maxSoul;
    [SerializeField] float _currentSoul;
    [SerializeField] float _currentShield;

    public float CurrentExp { get { return _currentExp; } set {  _currentExp = value; } }
    public float MaxSoul { get { return _maxSoul; }}
    public float CurrentSoul { get { return _currentSoul; }set { _currentSoul = value; } }
    public float CurrentShield { get { return _currentShield; } set { _currentShield = value; } }

    protected override void Start()
    {
        base.Start();
        _maxSoul = 100f;
        _currentSoul = 0;

    }
    public void OnHealHp(float amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + amount, MaxHp);
    }

   public void OnModifySoul(float amount)
    {
        _currentSoul = Mathf.Clamp(_currentSoul+amount, 0, MaxSoul);
    }
}
