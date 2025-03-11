using Data;
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

    #region Waepon Stat

    [SerializeField] float _weaponDamage;
    #endregion

    public float CurrentExp { get { return _currentExp; } set {  _currentExp = value; } }
    public float MaxSoul { get { return _maxSoul; }}
    public float CurrentSoul { get { return _currentSoul; }set { _currentSoul = value; } }
    public float CurrentShield { get { return _currentShield; } set { _currentShield = value; } }
    public float TotalAttack { get { return _currentAttack + _weaponDamage; } }

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

   public void OnModifySoul(float amount)
    {
        _currentSoul = Mathf.Clamp(_currentSoul+amount, 0, MaxSoul);
    }

    public void OnModifyEquipItem(int itemId)
    {
        _weaponDamage = 0;
        ItemData itemData = DataManager.Instance.GetItemData(itemId);
        WeaponData weaponData = itemData as WeaponData;

        if (weaponData !=null)
        {
            _weaponDamage = weaponData.damage;
        }
    }
}
