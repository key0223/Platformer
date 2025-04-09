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
        public Sprite itemIcon;
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
    }

    [Serializable]
    public class CharmData:ItemData
    {
        public int slotIndex;
        public int slotCost;
        public CharmType charmEffect;
        public float effectValue;
    }
    [Serializable]
    public class SpellData: ItemData
    {
        public string inputKey;
        public string keyDescription;
        public int damage;
        public Sprite decscrtiptionSprite;
    }

    #endregion

    [Serializable]
    public class StringData
    {
        public int stringId;
        [TextArea(2,5)]
        public string ko;
    }

    [Serializable]
    public class NpcData
    {
        public int npcId;
        public string displayName;
    }

    [Serializable]
    public class DialogueNode
    {
        public int nodeId;
        [TextArea(2, 5)]
        public string dialogueText;
    }
}