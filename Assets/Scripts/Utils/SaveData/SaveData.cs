using Data;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData 
{
   public PlayerSaveData playerSaveData = new PlayerSaveData();
   public InventorySaveData inventorySaveData = new InventorySaveData();
   public Dictionary<int,NpcProgressData> npcProgressDict = new Dictionary<int,NpcProgressData>();
}
