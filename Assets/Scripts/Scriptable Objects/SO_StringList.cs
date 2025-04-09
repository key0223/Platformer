using Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_StringList", menuName = "Scriptable Objects/String List")]
public class SO_StringList : ScriptableObject, ILoader<int, StringData>
{
    [SerializeField]
    public List<StringData> stringDatas = new List<StringData>();

    public Dictionary<int,StringData> MakeDict()
    {
        Dictionary<int, StringData> dict = new Dictionary<int, StringData>();

        foreach(StringData str in stringDatas)
        {
            dict.Add(str.stringId, str);
        }

        return dict;
    }
}
