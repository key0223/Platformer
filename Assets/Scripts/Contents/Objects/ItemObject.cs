using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ItemObject : MonoBehaviour
{
    [SerializeField] int _itemId;
    [SerializeField] SpriteRenderer _spriteRenderer;

    public int ItemId { get { return _itemId; } set { _itemId = value; } }

    public void Init(int itemId)
    {
        if (itemId != 0)
        {
            ItemId = itemId;
            ItemData data = DataManager.Instance.GetItemData(itemId);

            switch (data.itemType)
            {
                case ItemType.Coin:
                    _spriteRenderer.sprite = data.itemIcon;
                    break;
            }
        }
    }
}
