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
    [SerializeField]
    GameObject[] _charmCostSlots;
    [SerializeField] TextMeshProUGUI _itemDescText;
    [SerializeField] Image _itemIconImage;
    #endregion

    [Header("Equipped Slot Settings")]
    [SerializeField] string _charmEquippedSlotPrefabPath;
    [SerializeField] GameObject _equippedSlotParent;
    int _equippedSlotMaxCount = 8;

    [Header("Charm Cost Settings")]
    [SerializeField] string _charmCostSlotPrefabPath;
    [SerializeField] GameObject _costSlotParent;

    [Header("Selection Slot Settings")]
    [SerializeField] string _charmSlotPrefabPath;
    [SerializeField] GameObject _charmSlotParent;
    int _charmSlotCount = 30;

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    bool _openByBench = false;
    public bool OpenByBench { get { return _openByBench; } set {  _openByBench = value; } }

    public List<CharmSlot> EquippedCharms = new List<CharmSlot>();
    public List<CharmSlot> Charms = new List<CharmSlot>();
    public List<CharmCostSlot> CharmCostSlots = new List<CharmCostSlot>();

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
            List<Slot> firstRowColumns = _sections[0]._rows[0]._columns;

            GameObject equippedSlotObject = ResourceManager.Instance.Instantiate(_charmEquippedSlotPrefabPath, _equippedSlotParent.transform);
            CharmSlot charmSlot = equippedSlotObject.GetComponent<CharmSlot>();
            charmSlot.SlotIndex = 0;
            firstRowColumns.Add(charmSlot);

            EquippedCharms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.gameObject.SetActive(true);

            _initPos = charmSlot.transform;
            _highlighter.MoveToSlot(_initPos);
        }

        #endregion

        #region Charm Cost Slot Initialize

        foreach (Transform child in _costSlotParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < _playerMovement.Stat.CharmMaxCost; i++)
        {
            GameObject costSlotObject = ResourceManager.Instance.Instantiate(_charmCostSlotPrefabPath, _costSlotParent.transform);
            CharmCostSlot costSlot = costSlotObject.GetComponent<CharmCostSlot>();
            costSlot.SetSlotState(false);

            CharmCostSlots.Add(costSlot);
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
            _sections[1]._rows[0]._columns.Add(charmSlot);

            Charms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.CharmEquippedImage.gameObject.SetActive(false);
        }
        #endregion

        #region Arrow Slot Initialize
        for (int i = 0; i < _arrowSlot.Count; i++)
        {
            Slot slot = _arrowSlot[i];
            _sections[2]._rows[0]._columns.Add(slot);
        }
        #endregion

       
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

        if (!_openByBench) return;

        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._columns[_currentColumn];

        Item item = InventoryManager.Instance.GetItem(currentSlot.ItemId, ItemType.Charm);

        Charm charmItem = item as Charm;

        if (item == null) return;


        if (item.Equipped)
            item.Equipped = false;
        else
        {
            charmItem.Equipped = _playerMovement.Stat.CurrentAvailableCost >= charmItem.SlotCost;
        }

        RefreshUI();
        RefreshEquippedUI(item);
        RefreshCharmCostUI();
        _playerMovement.OnEquipItem();
    }
    #region Item Description UI
    void UpdateItemDescUI()
    {
        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._columns[_currentColumn];

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
                    CharmData charmData = data as CharmData;


                    if (charmData != null)
                    {
                        _itemNameText.text = data.itemName;
                        _itemDescText.text = data.itemDescription;
                        _itemIconImage.sprite = data.itemIcon;

                        _itemNameText.gameObject.SetActive(true);
                        _itemDescText.gameObject.SetActive(true);
                        _charmCostText.gameObject.SetActive(true);
                        _itemIconImage.gameObject.SetActive(true);

                        UpdateCharmCostSlotUI(charmData.slotCost);
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
                    CharmData charmData = data as CharmData;

                    if (charmData != null)
                    {
                        _itemNameText.text = data.itemName;
                        _itemDescText.text = data.itemDescription;
                        _itemIconImage.sprite = data.itemIcon;

                        _itemNameText.gameObject.SetActive(true);
                        _itemDescText.gameObject.SetActive(true);
                        _charmCostText.gameObject.SetActive(true);
                        _itemIconImage.gameObject.SetActive(true);

                        UpdateCharmCostSlotUI(charmData.slotCost);
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

    void UpdateCharmCostSlotUI(int cost)
    {
        foreach(GameObject slot in _charmCostSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < cost; i++)
        {
            _charmCostSlots[i].SetActive(true);
        }
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

            List<Slot> firstRowColumns = _sections[0]._rows[0]._columns;

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

    public void RefreshCharmCostUI()
    {
        if (Charms.Count == 0)
            return;

        List<Charm> charms = InventoryManager.Instance.Charms.Values.ToList();

        int totalCost = 0;
        foreach (Charm charm in charms)
        {
            if(charm.Equipped)
            {
                totalCost += charm.SlotCost;
            }
        }

        foreach (CharmCostSlot costSlot in CharmCostSlots)
        {
            // 일단 모두 비활성화
            costSlot.SetSlotState(false);
        }

        // TODO : 장착 가능한 cost가 부족하면 장착 실패 처리
        for (int i = 0; i < totalCost; i++)
        {
            CharmCostSlots[i].SetSlotState(true);
        }
    }
}
