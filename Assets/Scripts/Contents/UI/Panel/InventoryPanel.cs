using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

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

    [Header("Æ¯¼ö ½½·Ô")]
    [SerializeField] Slot _maskSlot;
    [SerializeField] Slot _weaponSlot;
    [SerializeField] Slot _amuletSlot;
    [SerializeField] Slot _spellSlot;

    protected override void Init()
    {
        _allSlots.Add(_maskSlot);
        _allSlots.Add(_weaponSlot);
        _allSlots.Add(_amuletSlot);
        _allSlots.Add(_spellSlot);
        AutoConnectSlots(_allSlots);
        ManualConnectSlots();

        _currentSlot = _allSlots.FirstOrDefault();
        MoveHighlighter(_currentSlot);

        InitItemDescUI();
    }

    void ManualConnectSlots()
    {
        _maskSlot.Left = null;
        _maskSlot.Right = _amuletSlot;
        _maskSlot.Up = null;
        _maskSlot.Down = _weaponSlot;

        _weaponSlot.Left = null;
        _weaponSlot.Right = _amuletSlot;
        _weaponSlot.Up = _maskSlot;
        _weaponSlot.Down = null;

        _amuletSlot.Left = _maskSlot;
        _amuletSlot.Right = _allSlots[0];
        _amuletSlot.Up = _maskSlot;
        _amuletSlot.Down = _spellSlot;

        _spellSlot.Left = _weaponSlot;
        _spellSlot.Right = _allSlots[0];
        _spellSlot.Up = _amuletSlot;
        _spellSlot.Down = null;

        _allSlots[0].Left = _amuletSlot;

    }
    public void SetCoinText(float amount)
    {
        int intOutput = Mathf.FloorToInt(amount);
        _coinText.text = intOutput.ToString();
    }

    public void RefreshUI()
    {
        if (InventoryManager.Instance.Items.Count == 0) return;

        List<Item> items = InventoryManager.Instance.Items.Values.Where(item => item.ItemType != Define.ItemType.None && item.ItemType != Define.ItemType.Map).ToList();

        for (int i = 0; i < items.Count; i++)
        {
            _allSlots[i].gameObject.SetActive(true);
            _allSlots[i].SetSlot(items[i]);
        }
    }

    protected override void OnSlotChanged()
    {
        UpdateItemDescUI();
    }

    #region Item Description UI

    void UpdateItemDescUI()
    {
        if (_currentSlot == null || _currentSlot.ItemId == 0)
        {
            InitItemDescUI();
            return;
        }

        ItemData data = DataManager.Instance.GetItemData(_currentSlot.ItemId);
        if (data != null)
        {
            _itemNameText.text = data.itemName;
            _itemDescText.text = data.itemDescription;
        }
        else
            InitItemDescUI();
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
