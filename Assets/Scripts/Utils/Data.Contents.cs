using System;
using UnityEngine;
using UnityEngine.UI;
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
        [TextArea(2, 5)]
        public string itemDescription;
    }

    [Serializable]
    public class CoinData:ItemData
    {
        public int coinValue;
    }

    [Serializable]
    public class WeaponData : ItemData
    {
        public float damage;
        public Sprite weaponSprite;
    }

    [Serializable]
    public class CharmData:ItemData
    {
        public int slotNumber;
        public CharmType charmEffect;
        public float effectValue;
    }
    [Serializable]
    public class SpellData: ItemData
    {
        public string inputKey;
        public string keyDescription;
        public int damage;
        public Sprite spellSprite;
        public Sprite DecscrtiptionSprite;
    }

    #endregion

    [Serializable]
    public class StringData
    {
        public int stringId;
        [TextArea(2,5)]
        public string ko;
    }
}