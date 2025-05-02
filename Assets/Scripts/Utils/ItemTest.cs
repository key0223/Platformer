using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    public int _itemId;

    public void AddItem()
    {
        if (_itemId == 0) return;

        Item item = Item.MakeItem(_itemId);
        InventoryManager.Instance.AddItem(item);
        Debug.Log("ITEM ADDED");


    }
}
