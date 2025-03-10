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
        public int SlotNumber;
        public CharmType CharmEffect;
        public float EffectValue;
    }

    #endregion

}