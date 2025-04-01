using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Data;

public class ItemPickUp : MonoBehaviour
{
    PlayerMovement _playerMovement;

    void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
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

                    float additaionalValue = (coinData.coinValue * _playerMovement.Stat.AdditionalCoin) / 100;
                    float finalValue = Mathf.Floor(additaionalValue * 10f) / 10f; // 소수점 한자리까지만 

                    InventoryManager.Instance.AddCoin(coinData.coinValue + finalValue);
                    ResourceManager.Instance.Destroy(collision.gameObject);
                    break;
            }

        }
    }
}
