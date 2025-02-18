using System;
using static Define;


namespace Data
{
    #region Item
    [Serializable]
    public class ItemData
    {
        public int itemId;
        public string itemName;
        public ItemType itemType;
        public string itemDescription;
    }

    [Serializable]
    public class CoinData:ItemData
    {
        public int coinValue;
    }
    #endregion

}