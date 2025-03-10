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
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    void Start()
    {
      
    }

    void Init()
    {
        _itemDict =  LoadScriptableObject<SO_ItemList,int,ItemData>("SO_ItemList").MakeDict();
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
