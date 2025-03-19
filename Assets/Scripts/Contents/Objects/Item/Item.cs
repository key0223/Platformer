using UnityEngine;
using Data;
using static Define;

public class Item : MonoBehaviour
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

            switch(data.itemType)
            {
                case ItemType.Coin:
                    CoinData coinData = data as CoinData;
                    _spriteRenderer.sprite = coinData.coinSprite;
                    break;
            }
        }
    }
}
