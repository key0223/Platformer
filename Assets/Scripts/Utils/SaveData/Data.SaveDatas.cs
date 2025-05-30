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
        public float CurrentHp;
        public float CurrentExp;
        public int CurrentShield;
    }

    [Serializable]
    public class InventorySaveData
    {
        public float currentCoin;

        public Dictionary<int,Item> items = new Dictionary<int,Item>();
        public Dictionary<int, Charm> charms = new Dictionary<int, Charm>();
    }

}
