using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Stat : MonoBehaviour
{
    protected List<Buff> _activeBuffs = new List<Buff>();
    [SerializeField] protected int _level = 1;
    [SerializeField] protected float _maxHp = 100;
    [SerializeField] protected float _attack = 10;
    [SerializeField] protected float _defense = 0;
    [SerializeField] protected float _needExpToNextLevel = 100;
    [SerializeField] protected float _currentHp;
    protected float _currentAttack;

    public float MaxHp { get { return _maxHp; } }
    public float Attack { get { return _attack; } }
    public float Defense { get { return _defense; } }
    public float NeedExpToNextLevel { get { return _needExpToNextLevel; } }
    public float CurrentHp { get { return _currentHp; } set { _currentHp = value; } }
    public float CurrentAttack { get { return _currentAttack; } set { _currentAttack = value; } }

    protected void Start()
    {
        _currentHp = _maxHp;
        _currentAttack = _attack;
    }

    public virtual void OnDamaged(float damage)
    {
        damage = Mathf.Max(damage - Defense, 0);
        CurrentHp = Mathf.Max(CurrentHp - damage, 0);
    }

}
