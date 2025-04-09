using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_NpcList", menuName = "Scriptable Objects/Npc List")]
public class SO_NPCList : ScriptableObject, ILoader<string, NpcData>
{
    [SerializeField]
    public List<NpcData> npcDatas = new List<NpcData>();

    public Dictionary<string,NpcData> MakeDict()
    {
         Dictionary<string, NpcData> dict = new Dictionary<string, NpcData>();

        foreach(NpcData npc in npcDatas)
        {
            dict.Add(npc.displayName, npc);
        }

        return dict;
    }
}
