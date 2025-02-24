using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterStat : Stat
{
    [SerializeField] float _dropSoulAmount;

    public float DropSoulAmount { get { return _dropSoulAmount; } }
}
