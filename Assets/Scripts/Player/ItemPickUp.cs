using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Data;

public class ItemPickUp : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();

        if(item != null )
        {
            ItemData data = DataManager.Instance.GetItemData(item.ItemId);

            if(data.itemType == ItemType.Coin)
            {
                CoinData coinData = (CoinData)data;

                InventoryManager.Instance.AddCoin(coinData.coinValue);

                ResourceManager.Instance.Destroy(collision.gameObject);
            }
        }
    }
}
