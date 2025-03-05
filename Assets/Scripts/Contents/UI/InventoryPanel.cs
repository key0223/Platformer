using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryPanel : PopupPanelBase
{
    [SerializeField] TextMeshProUGUI _coinText;

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

   
    protected override void Init()
    {
        _highlighter.MoveToSlot(_initPos);
    }

    public void SetCoinText(int amount)
    {
        _coinText.text = amount.ToString();
    }
}
