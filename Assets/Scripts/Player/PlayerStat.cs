using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField] float _currentExp;

    public float CurrentExp { get { return _currentExp; } set {  _currentExp = value; } }


}
