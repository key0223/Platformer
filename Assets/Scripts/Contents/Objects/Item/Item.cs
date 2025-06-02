using UnityEngine;
using Data;
using static Define;
using System;

[Serializable]
public class Item 
{
    protected int _itemId;
    protected int _count;

    protected bool _equipped;

    protected ItemType _itemType;
    public int ItemId { get { return _itemId; }set { _itemId = value; } }
    public int Count { get { return _count; } set { _count = value; } }
    public bool Equipped { get { return _equipped; }set { _equipped = value; } }
    public ItemType ItemType { get { return _itemType; } }

    public Item(ItemType itemType)
    {
        _itemType = itemType;
    }

    public static Item MakeItem(int itemId, int itemCount = 1)
    {
        Item item = null;
        ItemData itemData = DataManager.Instance.GetItemData(itemId);

        if(itemData == null)
            return null;

        switch(itemData.itemType)
        {
            case ItemType.None:
                item = new NormalItem(itemId);
                break;
            case ItemType.Weapon:
                item = new Weapon(itemId);
                break;
            case ItemType.Charm: 
                item = new Charm(itemId);
                break;
            case ItemType.Spell:
                break;
        }

        if (item != null)
        {
            item._itemId = itemId;
            item._count = itemCount;
            item.Equipped = false;
        }

        return item;
    }
    public virtual void Init(int itemId, int count = 1)
    {
        ItemData itemData  = DataManager.Instance.GetItemData(itemId);
        if (itemData != null )
        {
            _itemId = ItemId;
            _count = count;
            _equipped = false;
        }
    }
}

public class NormalItem :Item
{
    public NormalItem(int itemId) : base(ItemType.None)
    {
        Init(itemId);
    }
}
public class Weapon: Item
{
    float _damage;
   public float Damage { get { return _damage; }}

    public Weapon(int itemId) : base(ItemType.Weapon)
    {
        Init(itemId);
    }
    public override void Init(int itemId, int count=1)
    {
        ItemData itemData = DataManager.Instance.GetItemData(itemId);
       
        WeaponData weaponData  =  itemData as WeaponData;
        if (weaponData != null)
        {
            _itemId = itemId;
            _count = count;
            _equipped = false;
            _damage = weaponData.damage;
        }
    }
}

[Serializable]
public class Charm : Item
{
    int _slotIndex;
    int _slotCost;
    CharmType _charmEffectType;
    float _effectValue;

    public int SlotIndex { get { return _slotIndex; } }
    public int SlotCost {  get { return _slotCost; }}
    public CharmType CharmEffectType { get { return _charmEffectType; }}
    public float EffectValue { get { return _effectValue; }}

    public Charm (int itemId) :base(ItemType.Charm)
    {
        Init(itemId);
    }
    public override void Init(int itemId, int count=1)
    {
        ItemData itemData = DataManager.Instance.GetItemData(itemId);
        CharmData charmData = itemData as CharmData;
        if( charmData != null )
        {
            _itemId = itemId;
            _count = count;
            _equipped = false;

            _slotIndex = charmData.slotIndex;
            _slotCost = charmData.slotCost;
            _charmEffectType = charmData.charmEffect;
            _effectValue = charmData.effectValue;
        }
    }
}

