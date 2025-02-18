using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Buff
{
    public BuffType _buffType;
    public string _buffName;
    public string _buffStatType; // whether buff apply to hp, attack, defense atc
    public float _percentage;
    public float _duration;
    public float _currentTime;

    public void Init(BuffType buffType, string buffName,string buffStatType, float percentage, float duration)
    {
        _buffType = buffType;
        _buffName = buffName;
        _buffStatType = buffStatType;
        _percentage = percentage;
        _duration = duration;
        _currentTime = _duration;
    }

 

    void Deactivation()
    {
        //Destroy(gameObject);
    }
}
