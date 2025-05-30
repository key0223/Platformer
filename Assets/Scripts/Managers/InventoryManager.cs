using Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>,ISavable
{
    PlayerController _playerController;

    [SerializeField] HUDPanel HUD;
    #region Event
    public event Action<float> OnCoinChanged;
    #endregion
    public Dictionary<int, Item> Items { get; private set; } = new Dictionary<int, Item>();
    public Dictionary<int, Charm> Charms { get; private set; } = new Dictionary<int, Charm>();
    [SerializeField] float _coin;
    public float Coin
    {
        get { return _coin; }
        set
        {
            if (_coin == value) return;

            _coin = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _playerController = FindObjectOfType<PlayerController>();
        AddCoin(1000);
    }

    void OnEnable()
    {
        RegisterSave();
    }
    void OnDisable()
    {
        DeregisterSave();
    }
    public bool HasItem(int itemId)
    {
        return Items.ContainsKey(itemId) || Charms.ContainsKey(itemId);
    }
    #region Coin
    public void AddCoin(float amount)
    {
        if (amount > 0)
        {
            Coin += amount;
            OnCoinChanged?.Invoke(amount);
        }
    }
    public bool SpendCoin(int amount)
    {
        if (amount > 0 && Coin >= amount)
        {
            Coin -= amount;
            Debug.Log("아이템 구매 성공");
            return true;
        }

        Debug.Log("코인이 부족합니다");
        return false;
    }
    #endregion

    #region Item

    public void AddItem(Item item)
    {
        if (item.ItemType == ItemType.Charm)
        {
            Charm charm = item as Charm;

            Charms.Add(item.ItemId, charm);
        }
        else
        {
            Items.Add(item.ItemId, item);
        }

        // Refresh UI
        UIManager.Instance.PopupPanel.CharmPanel.RefreshUI();
        UIManager.Instance.PopupPanel.InvenPanel.RefreshUI();
    }

    public Item GetItem(int itemId, ItemType itemType = ItemType.Weapon)
    {
        Item item = null;
        if(itemType == ItemType.Charm)
        {
            Charm charm = null;
            Charms.TryGetValue(itemId, out charm);

            item = charm;
        }
        else
        {
            Items.TryGetValue(itemId, out item);
        }

        return item;
        
    }
    #endregion

    // Equip Item
    public void OnEquipItem()
    {
        _playerController.PlayerStat.OnRefreshEquipItem();
    }

    #region Save
    public void RegisterSave()
    {
        SaveLoadManager.Instance.Register(this);
    }
    public void DeregisterSave()
    {
        SaveLoadManager.Instance.Deregister(this);
    }

    public object CaptureData()
    {
        InventorySaveData data = new InventorySaveData();
        data.currentCoin = _coin;

        data.items = Items;
        data.charms = Charms;

        return data;
    }

    public void RestoreData(object loadeddata)
    {
        InventorySaveData data = loadeddata as InventorySaveData;

        if(data != null)
        {
            _coin = data.currentCoin;

            Items = data.items;
            Charms = data.charms;
        }
    }
    #endregion

}
