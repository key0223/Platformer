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

        return dict;

    }
}
