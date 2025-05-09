using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    Image _itemIconImage;

    [Space(10f)]
    [SerializeField]  int _itemId = 0;
    [SerializeField] bool _isArrow;
    [SerializeField] bool _arrowLeft;
    public int ItemId { get { return _itemId; } set { _itemId = value; } }
    public bool IsArrow { get { return _isArrow; }}
    public bool ArrowLeft { get { return _arrowLeft; }}

    void Awake()
    {
        _itemIconImage = GetComponentInChildren<Image>();
    }
  
    public void SetSlot(Item item)
    {
        if(item == null || item.ItemId == 0)
        {
            _itemIconImage.gameObject.SetActive(false);
        }
        else
        {
            ItemId = item.ItemId;

            ItemData itemData = DataManager.Instance.GetItemData(ItemId);

            if(itemData != null)
            {
                _itemIconImage.sprite = itemData.itemIcon;
                _itemIconImage.gameObject.SetActive(true);
            }
        }
    }
}
