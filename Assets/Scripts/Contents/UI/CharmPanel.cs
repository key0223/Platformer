using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharmPanel : PopupPanelBase
{
    [Header("Arrow Slot")]
    [SerializeField] List<Slot> _arrowSlot;

    [Space(10f)]
    #region Charm Description
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _charmCostText;
    [SerializeField] TextMeshProUGUI _itemDescText;
    [SerializeField] Image _itemIconImage;
    #endregion

    [Header("Equipped Slot Settings")]
    [SerializeField] string _charmEquippedSlotPrefabPath;
    [SerializeField] GameObject _equippedSlotParent;
    int _equippedSlotMaxCount = 8;

    [Header("Selection Slot Settings")]
    [SerializeField] string _charmSlotPrefabPath;
    [SerializeField] GameObject _charmSlotParent;
    int _charmSlotCount = 30;

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    public List<CharmSlot> EquippedCharms = new List<CharmSlot>();
    public List<CharmSlot> Charms = new List<CharmSlot>();

    protected override void Init()
    {
        _sections = new Section[3];

        for (int i = 0; i < _sections.Length; i++)
        {
            _sections[i] = new Section();

            SlotRow row = new SlotRow();
            _sections[i]._rows.Add(row);
        }

        _highlighter.MoveToSlot(_initPos);
        InitItemDescUI();

        Charms.Clear();

        #region Equipped Slot Initialize
        foreach (Transform child in _equippedSlotParent.transform)
            Destroy(child.gameObject);

        {
            List<Slot> firstRowColumns = _sections[0]._rows[0]._cloumns;

            GameObject equippedSlotObject = ResourceManager.Instance.Instantiate(_charmEquippedSlotPrefabPath, _equippedSlotParent.transform);
            CharmSlot charmSlot = equippedSlotObject.GetComponent<CharmSlot>();
            charmSlot.SlotIndex = 0;
            firstRowColumns.Add(charmSlot);

            EquippedCharms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.gameObject.SetActive(true);
        }
      
        #endregion

        #region Selection Slot Initialize
        foreach (Transform child in _charmSlotParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < _charmSlotCount; i++)
        {
            GameObject charmSlotObject = ResourceManager.Instance.Instantiate(_charmSlotPrefabPath, _charmSlotParent.transform);
            CharmSlot charmSlot = charmSlotObject.GetComponent<CharmSlot>();
            charmSlot.SlotIndex = i;
            _sections[1]._rows[0]._cloumns.Add(charmSlot);

            Charms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.CharmEquippedImage.gameObject.SetActive(false);
        }
        #endregion

        for (int i = 0; i < _arrowSlot.Count; i++)
        {
            Slot slot = _arrowSlot[i];
            _sections[2]._rows[0]._cloumns.Add(slot);
        }
        RefreshUI();

    }

    protected override void MoveSelection(int horizontal, int vertical, bool sectionMove)
    {
        base.MoveSelection(horizontal, vertical, sectionMove);
        UpdateItemDescUI();
    }

    protected override void SelectItem()
    {
        base.SelectItem();

        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn];

        Item item = InventoryManager.Instance.GetItem(currentSlot.ItemId, ItemType.Charm);

        if (item == null) return;

        item.Equipped = !item.Equipped;

        RefreshUI();
        RefreshEquippedUI(item);
        _playerMovement.OnEquipItem();
    }
    #region Item Description UI
    void UpdateItemDescUI()
    {
        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn];

        CharmSlot equippedSlot = currentSlot as CharmSlot;
        if (equippedSlot != null)
        {
            if(equippedSlot.SlotType == CharmSlotType.EquippedSlot)
            {
                if (!equippedSlot.IsEquipped)
                {
                    _itemNameText.text = "";
                    _charmCostText.text = "";

                    _itemNameText.gameObject.SetActive(false);
                    _charmCostText.gameObject.SetActive(false);
                    _itemIconImage.gameObject.SetActive(false);

                    _itemDescText.text = "장착된 부적이 없습니다. \n아래에서 부적을 선택하여 장착하고 그 효과를 활성화하십시오.";
                }
                else
                {
                    ItemData data = DataManager.Instance.GetItemData(currentSlot.ItemId);

                    if (data != null)
                    {
                        _itemNameText.text = data.itemName;
                        _itemDescText.text = data.itemDescription;
                        _itemIconImage.sprite = data.itemIcon;

                        _itemNameText.gameObject.SetActive(true);
                        _itemDescText.gameObject.SetActive(true);
                        _charmCostText.gameObject.SetActive(true);
                        _itemIconImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        InitItemDescUI();
                    }
                }
            }
            else
            {
                if(equippedSlot.ItemId ==0)
                {
                    InitItemDescUI();
                }
                else
                {
                    ItemData data = DataManager.Instance.GetItemData(currentSlot.ItemId);

                    if (data != null)
                    {
                        _itemNameText.text = data.itemName;
                        _itemDescText.text = data.itemDescription;
                        _itemIconImage.sprite = data.itemIcon;

                        _itemNameText.gameObject.SetActive(true);
                        _itemDescText.gameObject.SetActive(true);
                        _charmCostText.gameObject.SetActive(true);
                        _itemIconImage.gameObject.SetActive(true);
                    }
                }
            }
           
        }
    }
    void InitItemDescUI()
    {
        _itemNameText.text = "";
        _charmCostText.text = "";
        _itemDescText.text = "";

        _charmCostText.gameObject.SetActive(false);
        _itemIconImage.gameObject.SetActive(false);
    }
    #endregion

    // Selection UI
    public void RefreshUI()
    {
        if (Charms.Count == 0)
            return;

        List<Charm> charms = InventoryManager.Instance.Charms.Values.ToList();

        foreach(Charm charm in charms)
        {
            Charms[charm.SlotIndex].SetSlot(charm);
        }
    }

   
    // Equipped UI
    public void RefreshEquippedUI(Item item)
    {
        if(item.Equipped)
        {
            EquippedCharms[EquippedCharms.Count - 1].SetSlot(item);

            List<Slot> firstRowColumns = _sections[0]._rows[0]._cloumns;

            GameObject equippedSlotObject = ResourceManager.Instance.Instantiate(_charmEquippedSlotPrefabPath, _equippedSlotParent.transform);
            CharmSlot charmSlot = equippedSlotObject.GetComponent<CharmSlot>();
            charmSlot.SlotIndex = Mathf.Max(0, EquippedCharms.Count - 1);
            firstRowColumns.Add(charmSlot);

            EquippedCharms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < EquippedCharms.Count; i++)
            {
                if (EquippedCharms[i].ItemId != item.ItemId)
                    continue;

                Destroy(EquippedCharms[i].gameObject);
                EquippedCharms.RemoveAt(i);
            }
        }
       
    }
}
