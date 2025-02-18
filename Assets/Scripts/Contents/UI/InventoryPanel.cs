using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPanel : PopupPanelBase
{
    // Highlighter 
    [SerializeField] Transform _initPos;

    [Space(10f)]
    [SerializeField] TextMeshProUGUI _coinText;
    protected override void InitSlotList()
    {
        base.InitSlotList();
        _highlighter.MoveToSlot(_initPos);
    }

    public void SetCoinText(int amount)
    {
        _coinText.text = amount.ToString();
    }
}
