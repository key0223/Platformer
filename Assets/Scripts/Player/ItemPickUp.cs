using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Data;

public class ItemPickUp : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ItemObject item = collision.gameObject.GetComponent<ItemObject>();

        if (item != null)
        {
            ItemData data = DataManager.Instance.GetItemData(item.ItemId);

            switch (data.itemType)
            {
                case ItemType.Coin:
                    CoinData coinData = data as CoinData;

                    InventoryManager.Instance.AddCoin(coinData.coinValue);
                    ResourceManager.Instance.Destroy(collision.gameObject);
                    break;
            }

        }
    }
}
