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

    [Serializable]
    public class MapData : ItemData
    {
        public SceneName sceneName;
        public string miniMapPrefabPath;
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
        public string nodeId;
        [TextArea(2,20)]
        public string dialogueText;
        public string conditionKey; // 조건 키
        public bool hasFollowingDialogue;
        [HideInInspector]
        public string followingDialogueId;
        /*
         * conversation:1 -> 대화 1번 함
         * always -> 항상 표시 (조건 없음)
        */
    }

    [Serializable]
    public class ShopData
    {
        [ItemIdName]
        public int itemId;
        public int price;
        [TextArea(2, 5)]
        public string npcItemDesc;
    }
}