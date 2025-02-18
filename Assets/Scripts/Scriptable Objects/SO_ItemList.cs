using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

[CreateAssetMenu(fileName ="SO_ItemList",menuName ="Scriptable Objects/Item List")]
public class SO_ItemList : ScriptableObject, ILoader<int,ItemData>
{
    [SerializeField]
    public List<CoinData> coinDatas;

    public Dictionary<int,ItemData> MakeDict()
    {
        Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();

        foreach(ItemData item in coinDatas)
        {
            dict.Add(item.itemId, item);
        }

        return dict;

    }
}
