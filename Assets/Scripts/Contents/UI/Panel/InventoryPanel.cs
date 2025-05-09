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

    [SerializeField] Slot _weaponSlot;

    List<Slot> _belongings = new List<Slot>();
    protected override void Init()
    {
        _highlighter.MoveToSlot(_initPos);

        InitItemDescUI();
        InitializeBelongingsList();
    }

    void InitializeBelongingsList()
    {
        Section section = _sections[1];

        for (int i = 0; i< section._rows.Count; i++)
        {
            SlotRow row = section._rows[i];

            for (int j = 0; j < row._columns.Count; j++)
            {
                Slot slot = row._columns[j];
                _belongings.Add(slot);
                slot.gameObject.SetActive(false);
            }
        }
    }
    protected override void MoveSelection(int horizontal, int vertical, bool sectionMove)
    {
        base.MoveSelection(horizontal, vertical, sectionMove);
        UpdateItemDescUI();
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
            _belongings[i].gameObject.SetActive(true);
            _belongings[i].SetSlot(items[i]);
        }
    }

    #region Item Description UI
    void UpdateItemDescUI()
    {
        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._columns[_currentColumn];

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
