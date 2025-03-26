using UnityEngine;
using Data;
using static Define;
using System.Security.Cryptography.X509Certificates;

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

    public virtual void Init(int itemId, int count)
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

public class Weapon: Item
{
    float _damage;
   public float Damage { get { return _damage; }}

    public override void Init(int itemId, int count)
    {
        ItemData itemData = DataManager.Instance.GetItemData(itemId);
        WeaponData weaponData  =  itemData as WeaponData;
        if (weaponData != null)
        {
            _itemId = itemId;
            _count = count;
            _equipped = false;
            _itemType = ItemType.Weapon;

            _damage = weaponData.damage;
        }
    }
}

public class Charm : Item
{
    int _slotCost;
    CharmType _charmEffectType;
    float _effectValue;

    public int SlotCost {  get { return _slotCost; }}
    public CharmType CharmEffectType { get { return _charmEffectType; }}
    public float EffectValue { get { return _effectValue; }}

    public override void Init(int itemId, int count)
    {
        ItemData itemData = DataManager.Instance.GetItemData(itemId);
        CharmData charmData = itemData as CharmData;
        if( charmData != null )
        {
            _itemId = itemId;
            _count = count;
            _equipped = false;
            _itemType= ItemType.Charm;

            _slotCost = charmData.slotCost;
            _charmEffectType = charmData.charmEffect;
            _effectValue = charmData.effectValue;
        }
    }
}