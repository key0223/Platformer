using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    [SerializeField] HUDPanel HUD;
    #region Event
    public event Action<float> OnCoinChanged;
    #endregion
    public Dictionary<int, Item> Items { get; } = new Dictionary<int, Item>();
    public Dictionary<int, Charm> Charms { get; } = new Dictionary<int, Charm>();
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
            return true;
        }
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

        // TODO: Inventory Refresh UI 

        UIManager.Instance.PopupPanel.CharmPanel.RefreshUI();
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
}
