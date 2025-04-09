using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public interface ILoader<key,Value>
{
    Dictionary<key, Value> MakeDict(); // 사용할 때 타입 지정
}

public class DataManager : SingletonMonobehaviour<DataManager>
{
    Dictionary<int, ItemData> _itemDict;
    Dictionary<int, StringData> _stringDict;
    Dictionary<string, NpcData> _npcDict;
    Dictionary<int, DialogueNode> _dialogueDict;
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Init()
    {
        _itemDict =  LoadScriptableObject<SO_ItemList,int,ItemData>("SO_ItemList").MakeDict();
        _stringDict = LoadScriptableObject<SO_StringList, int, StringData>("SO_StringList").MakeDict();
        _npcDict = LoadScriptableObject<SO_NPCList, string, NpcData>("SO_NPCList").MakeDict();
        _dialogueDict = LoadScriptableObject<SO_DialogueList, int, DialogueNode>("SO_DialogueList").MakeDict();
    }
   
    public ItemData GetItemData(int itemId)
    {
        ItemData itemData = null;
        if (_itemDict.TryGetValue(itemId, out itemData))
        {
            return itemData;
        }
        else return null;
    }

    public StringData GetStringData(int id) 
    {
        StringData stringData = null;
        if (_stringDict.TryGetValue(id, out stringData))
        {
            return stringData;
        }
        else return null;
    }
    /*
     * Loader = Generic parameter
     * Loader must implement ILoader<key,Value>
    */
    Loader LoadScriptableObject<Loader,Key,Value>(string path) where Loader : ILoader<Key,Value>
    {
        ScriptableObject scriptableObject = ResourceManager.Instance.Load<ScriptableObject>($"Scriptable Objects/{path}");

        if (scriptableObject is Loader loader)
        {
            return loader;
        }
        return default(Loader);
    }
   
}
