using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Image _itemIconImage;
    [SerializeField] TextMeshProUGUI _itemCostText;
    [SerializeField] Image _purchasedImage;

    public int ItemId { get; private set; }
    public int ItemCost { get; private set; }

    bool _isPurchased = false;

    public bool IsPurchased 
    { 
        get {  return _isPurchased; }
        set 
        {
            if(_isPurchased == value) return;
            _isPurchased = value;
            UpdateItemUIState(value);
        }
    }

    public void Init(int itemId,int itemCost)
    {
        ItemId = itemId;
        ItemCost = itemCost;
        SetUI();
    }
    void SetUI()
    {
        _itemIconImage.sprite = DataManager.Instance.GetItemData(ItemId).itemIcon;
        _itemCostText.text = ItemCost.ToString();
        _purchasedImage.gameObject.SetActive(false);
    }

    void UpdateItemUIState(bool purchased)
    {
        if( purchased )
        {
            _purchasedImage.gameObject.SetActive(true);
        }
    }
}
