using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryPanel : PopupPanelBase
{
    [SerializeField] TextMeshProUGUI _coinText;

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    #region Item Description UI
    [Space(10f)]
    [Header("Item Description")]
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _keyDescText;
    [SerializeField] TextMeshProUGUI _itemDescText;
    #endregion

    [SerializeField] Slot _weaponSlot;

    protected override void Init()
    {
        _highlighter.MoveToSlot(_initPos);

        InitItemDescUI();
    }
    protected override void MoveSelection(int horizontal, int vertical, bool sectionMove)
    {
        base.MoveSelection(horizontal, vertical, sectionMove);
        UpdateItemDescUI();
    }
    public void SetCoinText(int amount)
    {
        _coinText.text = amount.ToString();
    }

    #region Item Description UI
    void UpdateItemDescUI()
    {
        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn];

        ItemData data = DataManager.Instance.GetItemData(currentSlot.ItemId);
        if (data != null)
        {
            _itemNameText.text = data.itemName;
            _itemDescText.text = data.itemDescription;
        }
        else
        {
            InitItemDescUI();
        }
    }

    void InitItemDescUI()
    {
        _itemNameText.text = "";
        _keyDescText.text = "";
        _itemDescText.text = "";
    }
    #endregion

    #region Equip Item
    public int GetEquippedWeaponDamage()
    {
        ItemData itemData = DataManager.Instance.GetItemData(_weaponSlot.ItemId);
        WeaponData weaponData = itemData as WeaponData;

        if (weaponData !=null)
        {
            return (int)weaponData.damage;
        }

        return 0;
    }
    #endregion
}
