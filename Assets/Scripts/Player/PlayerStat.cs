using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;
public class PlayerStat : Stat
{
    [SerializeField] float _currentExp;
    [SerializeField] float _maxSoul;
    [SerializeField] float _currentSoul;
    [SerializeField] float _currentShield;

    

    #region Item Stat

    [SerializeField] float _additionalHp;
    [SerializeField] float _weaponDamage;
    [SerializeField] float _additionalSoul;
    [SerializeField] float _additionalCoin;
    #endregion

    int _charmMaxCost = 3;

    public float CurrentExp { get { return _currentExp; } set {  _currentExp = value; } }
    public float MaxSoul { get { return _maxSoul; }}
    public float CurrentSoul { get { return _currentSoul; }set { _currentSoul = value; } }
    public float CurrentShield { get { return _currentShield; } set { _currentShield = value; } }

    public float TotalMaxHp { get { return _maxHp + _additionalHp; } }
    public float TotalAttack { get { return _currentAttack + _weaponDamage; } }

    public float AdditionalSoul { get { return _additionalSoul; }}
    public float AdditionalCoin { get { return _additionalCoin; }}

    public int CharmMaxCost { get { return _charmMaxCost; }}
    public int CurrentAvailableCost
    {
        get
        {
            int usingCost = 0;
            foreach (Charm charm in InventoryManager.Instance.Charms.Values)
            {
                if (charm.Equipped == false)
                    continue;

                usingCost += charm.SlotCost;
            }

            int remainCost = _charmMaxCost - usingCost;
            return remainCost;
        }
    }

    protected override void Start()
    {
        base.Start();
        _maxSoul = 100f;
        _currentSoul = 0;

        // Initial Player stat settings
        _weaponDamage = UIManager.Instance.PopupPanel.InvenPanel.GetEquippedWeaponDamage();
    }
    public void OnHealHp(float amount)
    {
        CurrentHp = Mathf.Min(CurrentHp + amount, MaxHp);
    }

   public void OnRefreshSoul(float amount)
    {
        _currentSoul = Mathf.Clamp(_currentSoul+amount, 0, MaxSoul);
    }

    public void OnRefreshEquipItem()
    {
        _additionalHp = 0;
        _weaponDamage = 0;
        _additionalSoul = 0;
        _additionalCoin = 0;
        foreach(Item item in InventoryManager.Instance.Items.Values)
        {
            if (item.Equipped == false)
                continue;

           if(item.ItemType == ItemType.Weapon)
            {
                _weaponDamage += ((Weapon)item).Damage;
            }
        }

        foreach(Charm charm in InventoryManager.Instance.Charms.Values)
        {
            if(charm.Equipped == false)
                continue;

            switch (charm.CharmEffectType)
            {
                case CharmType.Attack:
                    float additionalDamage = (_attack + _weaponDamage) * charm.EffectValue / 100;
                    _weaponDamage += additionalDamage;

                    break;
                case CharmType.Hp:
                    float additionalValue = _maxHp * charm.EffectValue / 100;
                    _additionalHp += additionalValue;
                    break;
                case CharmType.Soul:
                    _additionalSoul += charm.EffectValue;
                    break;
                case CharmType.Orbs:
                    _additionalCoin += charm.EffectValue;
                    break;
            }
        }
      
    }
}
