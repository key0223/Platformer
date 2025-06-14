using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerSaveData
    {
        public string currentScene;
        public float posX, posY;

        // Stat
        public int level;
        public float currentHp;
        public float currentExp;
        public float currentSoul;
        public int currentShield;
    }

    [Serializable]
    public class InventorySaveData
    {
        public float currentCoin;

        public Dictionary<int,Item> items = new Dictionary<int,Item>();
        public Dictionary<int, Charm> charms = new Dictionary<int, Charm>();
    }

    [Serializable]
    public class NpcProgressData
    {
        public int npcId;
        public bool hasMet;
    }

    [Serializable]
    public class MiniMapSaveData
    {
        public int itemId;
        public List<Vector2IntSerializable> visitedBlocks = new List<Vector2IntSerializable>();
    }
}
