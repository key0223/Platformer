using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using static Define;

[CreateAssetMenu(fileName ="SO_ItemList",menuName ="Scriptable Objects/Item List")]
public class SO_ItemList : ScriptableObject, ILoader<int,ItemData>
{
    [SerializeField]
    public List<CoinData> coinDatas;
    [SerializeField]
    public List<WeaponData> weaponDatas;
    [SerializeField]
    public List<CharmData> charmDatas;
    [SerializeField]
    public List<SpellData> spellDatas;
    [SerializeField]
    public List<MiniMapData> miniMapDatas;
    [SerializeField]
    public List<ItemData> itemDatas;

    public Dictionary<int,ItemData> MakeDict()
    {
        Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();

        foreach(ItemData item in coinDatas)
        {
            item.itemType = ItemType.Coin;
            dict.Add(item.itemId, item);
        }

        foreach(ItemData item in weaponDatas)
        {
            item.itemType = ItemType.Weapon;
            dict.Add(item.itemId, item);
        }
        foreach (ItemData item in charmDatas)
        {
            item.itemType = ItemType.Charm;
            dict.Add(item.itemId, item);
        }
        foreach (ItemData item in spellDatas)
        {
            item.itemType = ItemType.Spell;
            dict.Add(item.itemId, item);
        }
        foreach(ItemData item in miniMapDatas)
        {
            item.itemType = ItemType.Map;
            dict.Add(item.itemId, item);
        }

        foreach(ItemData item in itemDatas)
        {
            item.itemType = ItemType.None;
            dict.Add(item.itemId, item);
        }
        return dict;

    }
}
