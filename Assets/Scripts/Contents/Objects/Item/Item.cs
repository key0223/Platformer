using UnityEngine;
using Data;

public class Item : MonoBehaviour
{
    [SerializeField] int _itemId;


    public int ItemId { get { return _itemId; } set { _itemId = value; } }

    void Start()
    {
        if (ItemId != 0)
        {
            Init(ItemId);
        }
        
    }

    public void Init(int itemId)
    {
        if (itemId != 0)
        {
            ItemId = itemId;
            ItemData data = DataManager.Instance.GetItemData(itemId);

        }
    }
}
