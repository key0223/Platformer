using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharmPanel : PopupPanelBase
{
    [Header("특수 슬롯")]
    [SerializeField] Slot _leftArrow;
    [SerializeField] Slot _rightArrow;

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

    PlayerController _playerController;

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    bool _openByBench = false;
    public bool OpenByBench { get { return _openByBench; } set {  _openByBench = value; } }

    [SerializeField] List<CharmSlot> _charms = new List<CharmSlot>();

    
    public List<CharmSlot> EquippedCharms = new List<CharmSlot>();
    public List<CharmCostSlot> CharmCostSlots = new List<CharmCostSlot>();

    protected override void Init()
    {
        _playerController = FindObjectOfType<PlayerController>();

        // Equipped Slot Initialize
        foreach (Transform child in _equippedSlotParent.transform)
            Destroy(child.gameObject);

        {
            GameObject equippedSlotObject = ResourceManager.Instance.Instantiate(_charmEquippedSlotPrefabPath, _equippedSlotParent.transform);
            CharmSlot charmSlot = equippedSlotObject.GetComponent<CharmSlot>();
            charmSlot.SlotIndex = 0;
            _allSlots.Add(charmSlot);

            EquippedCharms.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.gameObject.SetActive(true);
        }

        //Charm Cost Slot Initialize

        foreach (Transform child in _costSlotParent.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < _playerController.PlayerStat.CharmMaxCost; i++)
        {
            GameObject costSlotObject = ResourceManager.Instance.Instantiate(_charmCostSlotPrefabPath, _costSlotParent.transform);
            CharmCostSlot costSlot = costSlotObject.GetComponent<CharmCostSlot>();
            costSlot.SetSlotState(false);

            CharmCostSlots.Add(costSlot);
        }

        // Selection Slot Initialize

        for (int i = 0; i < _charms.Count; i++)
        {
            CharmSlot charmSlot = _charms[i];
            charmSlot.SlotIndex = i;

            RectTransform rectTransform = charmSlot.GetComponent<RectTransform>();

            _allSlots.Add(charmSlot);

            charmSlot.CharmIconImage.gameObject.SetActive(false);
            charmSlot.CharmEquippedImage.gameObject.SetActive(false);
        }

        RefreshUI();

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_charmSlotParent.transform); // 레이아웃 강제 갱신
        AutoConnectSlots(_allSlots);
        ManualConnectSlots();

        _allSlots.Add(_leftArrow);
        _allSlots.Add(_rightArrow);

        _currentSlot = _allSlots.FirstOrDefault();
        MoveHighlighter(_currentSlot);
        InitItemDescUI();
    }

    // Since using Grid Layout Group, have to use position instead of anchoredPosition
    protected override Slot FindClosestSlot(Slot from, List<Slot> slots, Vector2 dir, float maxAngle)
    {
        Slot closest = null;
        float minDist = float.MaxValue;
        Vector2 fromPos = from.transform.position;

        foreach (Slot slot in slots)
        {
            if (slot == from) continue;

            Vector2 toPos = slot.transform.position;
            Vector2 diff = toPos - fromPos;
            float angle = Vector2.Angle(dir, diff);
            if (angle < maxAngle)
            {
                float dist = diff.magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = slot;
                }
            }
        }

        return closest;
    }
    void ManualConnectSlots()
    {
        _allSlots[0].Left = _leftArrow;
        _allSlots[0].Right = _rightArrow;

        _charms[0].Left = _leftArrow;

        UpdateArrowSlot();
    }

   
    protected override void OnSlotChanged()
    {
        UpdateItemDescUI();

    }
    protected override void SelectItem()
    {
        base.SelectItem();

        if (!_openByBench) return;

        Item item = InventoryManager.Instance.GetItem(_currentSlot.ItemId, ItemType.Charm);

        Charm charmItem = item as Charm;

        if (item == null) return;


        if (item.Equipped)
            item.Equipped = false;
        else
        {
            charmItem.Equipped = _playerController.PlayerStat.CurrentAvailableCost >= charmItem.SlotCost;
        }

        RefreshUI();
        RefreshEquippedUI(item);
        RefreshCharmCostUI();
        InventoryManager.Instance.OnEquipItem();
    }

    public override void UpdateArrowSlot()
    {
        _leftArrow.Left = null;
        _leftArrow.Right = _allSlots[0];
        _leftArrow.Up = _rightArrow;
        _leftArrow.Down = _rightArrow;

        _rightArrow.Left = _allSlots[0];
        _rightArrow.Right = null;
        _rightArrow.Up = _leftArrow;
        _rightArrow.Down = _leftArrow;
    }
    #region Item Description UI
    void UpdateItemDescUI()
    {
        if (_currentSlot == null || _currentSlot.ItemId == 0)
        {
            InitItemDescUI();
            return;
        }


        CharmSlot equippedSlot = _currentSlot as CharmSlot;
        if (equippedSlot != null)
        {
            if (equippedSlot.SlotType == CharmSlotType.EquippedSlot)
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
                    ItemData data = DataManager.Instance.GetItemData(_currentSlot.ItemId);
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
                if (equippedSlot.ItemId == 0)
                {
                    InitItemDescUI();
                }
                else
                {
                    ItemData data = DataManager.Instance.GetItemData(_currentSlot.ItemId);
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
        if (_charms.Count == 0)
            return;

        List<Charm> charms = InventoryManager.Instance.Charms.Values.ToList();

        foreach(Charm charm in charms)
        {
            _charms[charm.SlotIndex].SetSlot(charm);
        }
    }


    // Equipped UI
    public void RefreshEquippedUI(Item item)
    {
        if (item.Equipped)
        {
            EquippedCharms[EquippedCharms.Count - 1].SetSlot(item);

            GameObject equippedSlotObject = ResourceManager.Instance.Instantiate(_charmEquippedSlotPrefabPath, _equippedSlotParent.transform);
            CharmSlot newEquippedCharmSlot = equippedSlotObject.GetComponent<CharmSlot>();
            newEquippedCharmSlot.SlotIndex = Mathf.Max(0, EquippedCharms.Count - 1);
            _allSlots.Add(newEquippedCharmSlot);

            // Update prev EquippedCharm[EquippedCharms.Count - 1] direction
            EquippedCharms[EquippedCharms.Count - 1].Right = newEquippedCharmSlot;

            newEquippedCharmSlot.Left = EquippedCharms[EquippedCharms.Count-1];
            newEquippedCharmSlot.Down = _charms[0];

            EquippedCharms.Add(newEquippedCharmSlot);

            newEquippedCharmSlot.CharmIconImage.gameObject.SetActive(false);
            newEquippedCharmSlot.gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < EquippedCharms.Count; i++)
            {
                if (EquippedCharms[i].ItemId != item.ItemId)
                    continue;

                _allSlots.Remove(EquippedCharms[i]);

                // 마지막 슬롯일 때
                if(i+1 == EquippedCharms.Count-1)
                {
                    _currentSlot = EquippedCharms[EquippedCharms.Count-1];

                    _currentSlot.Left = _leftArrow;
                    _currentSlot.Right = null;
                    _currentSlot.Up = null;
                    _currentSlot.Down = _charms[0];
                }
                else
                {
                    _currentSlot = EquippedCharms[i + 1];

                    _currentSlot.Left = _leftArrow;
                    _currentSlot.Right = EquippedCharms[i + 2];
                    _currentSlot.Up = null;
                    _currentSlot.Down = _charms[0];
                }

                Destroy(EquippedCharms[i].gameObject);
                EquippedCharms.RemoveAt(i);

                MoveHighlighter(_currentSlot);
            }
        }
    }

    public void RefreshCharmCostUI()
    {
        if (_charms.Count == 0)
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
