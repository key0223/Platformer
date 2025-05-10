using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBarSlot : Slot
{
    [SerializeField] Image _markerImage;

    public void Init(int itemId)
    {
        ItemData data = DataManager.Instance.GetItemData(itemId);
        if (data != null)
        {
            _markerImage.sprite = data.itemIcon;
        }
    }
}
