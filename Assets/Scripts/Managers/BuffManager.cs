using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BuffManager : SingletonMonobehaviour<BuffManager>
{
    List<Buff> _activeBuffs = new List<Buff>();
    PlayerStat _playerStat;
    protected override void Awake()
    {
        base.Awake();
        _playerStat = FindObjectOfType<PlayerStat>();
   
    }

    private void Update()
    {
        if (_activeBuffs.Count <= 0) return;

        for (int i = _activeBuffs.Count - 1; i >= 0; i--)
        {
            _activeBuffs[i]._currentTime -= Time.deltaTime;
            if (_activeBuffs[i]._currentTime <= 0)
            {
                RemoveBuff(_activeBuffs[i]);
            }
        }
    }

    public void AddBuff(Buff newBuff)
    {
        Buff existingBuff = _activeBuffs.Find(b => b._buffName == newBuff._buffName);
        if (existingBuff != null)
        {
            existingBuff._currentTime = newBuff._duration;
            return;
        }

        _activeBuffs.Add(newBuff);
        UpdateStat(newBuff);
    }
    protected void RemoveBuff(Buff buff)
    {
        _activeBuffs.Remove(buff);
        UpdateStat(buff);
    }

    #region Update Stat
    protected void UpdateStat(Buff buff)
    {
        switch (buff._buffStatType)
        {
            case "Attack":
                {
                    ModifyAttack();
                }
                break;
            case "Defense":
                break;
        }
    }

    protected void ModifyAttack()
    {
        float buffPercentage = 0f;
        float debuffPercentage = 0f;

        foreach (Buff buff in _activeBuffs)
        {
            if (buff._buffStatType.Equals("Attack"))
            {
                float percentage = buff._percentage / 100f;
                if (buff._buffType == BuffType.Buff)
                {
                    buffPercentage += percentage;
                }
                else if (buff._buffType == BuffType.Debuff)
                {
                    debuffPercentage += percentage;
                }
            }
        }

        float modifiedAttackPower = _playerStat.Attack * (1 + buffPercentage - debuffPercentage);

        _playerStat.CurrentAttack = Mathf.Max(modifiedAttackPower, 0);

    }
    #endregion
}
