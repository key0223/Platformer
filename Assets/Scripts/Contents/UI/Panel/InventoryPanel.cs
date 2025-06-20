using Data;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Define;

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

    [Header("Ư�� ����")]
    [SerializeField] Slot _maskSlot;
    [SerializeField] Slot _weaponSlot;
    [SerializeField] Slot _amuletSlot;
    [SerializeField] Slot _spellSlot;
    [Space(10f)]
    [SerializeField] Slot _leftArrow;
    [SerializeField] Slot _rightArrow;

    protected override void Init()
    {
        _uiType = UIType.Inventory;
        AutoConnectSlots(_allSlots);
        ManualConnectSlots();
        SetAllSlotsDisable();

        _allSlots.Add(_maskSlot);
        _allSlots.Add(_weaponSlot);
        _allSlots.Add(_amuletSlot);
        _allSlots.Add(_spellSlot);
        _allSlots.Add(_leftArrow);
        _allSlots.Add(_rightArrow);

        _currentSlot = _allSlots.FirstOrDefault();
        MoveHighlighter(_currentSlot);

        InitItemDescUI();
    }

    void SetAllSlotsDisable()
    {
        foreach (Slot slot in _allSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    void ManualConnectSlots()
    {
        _maskSlot.Left = _leftArrow;
        _maskSlot.Right = _amuletSlot;
        _maskSlot.Up = null;
        _maskSlot.Down = _weaponSlot;

        _weaponSlot.Left = _leftArrow;
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
        _allSlots[_allSlots.Count - 1].Right = _rightArrow;

       UpdateArrowSlot();
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

    public override void UpdateArrowSlot()
    {
        _leftArrow.Left = null;
        _leftArrow.Right = _maskSlot;
        _leftArrow.Up = _rightArrow;
        _leftArrow.Down = _rightArrow;

        _rightArrow.Left = _allSlots[_allSlots.Count - 1];
        _rightArrow.Right = null;
        _rightArrow.Up = _leftArrow;
        _rightArrow.Down = _leftArrow;
    }
    #region  UI

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
}
