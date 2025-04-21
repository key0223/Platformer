using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_ShopList", menuName = "Scriptable Objects/Shop List")]
public class SO_ShopList : ScriptableObject
{
    [SerializeField]
    public List<ShopData> itemList;
}
