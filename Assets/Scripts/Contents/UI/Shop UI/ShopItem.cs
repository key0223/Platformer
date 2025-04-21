using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Image _itemIconImage;
    [SerializeField] TextMeshProUGUI _itemCostText;

    public int ItemId { get; private set; }
    public float ItemCost { get; private set; }

    public void Init(int itemId,float itemCost)
    {
        ItemId = itemId;
        ItemCost = itemCost;
        SetUI();
    }
    void SetUI()
    {
        _itemIconImage.sprite = DataManager.Instance.GetItemData(ItemId).itemIcon;
        _itemCostText.text = ItemCost.ToString();
    }


}
