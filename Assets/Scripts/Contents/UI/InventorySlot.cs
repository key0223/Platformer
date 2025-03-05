using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : Slot
{

    [SerializeField] int _currentItemId = 0; // The item id attached this slot
    public int CurrentItemId { get { return _currentItemId; } set { _currentItemId = value; } }


}
