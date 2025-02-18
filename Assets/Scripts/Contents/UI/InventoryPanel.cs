using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : PopupPanelBase
{
    // Highlighter 
    [SerializeField] Transform _initPos;

    protected override void InitSlotList()
    {
        base.InitSlotList();
        _highlighter.MoveToSlot(_initPos);
    }
}
