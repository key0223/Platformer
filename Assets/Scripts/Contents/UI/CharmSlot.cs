using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CharmSlot : Slot
{
    [SerializeField] CharmSlotType _slotType;
    [SerializeField] bool _isEquipped = false;

    public CharmSlotType SlotType { get { return _slotType; } }
    public bool IsEquipped { get { return _isEquipped; } }
}
