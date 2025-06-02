using Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : SingletonMonobehaviour<SaveLoadManager>
{
    string _savePath;
    string _fileName = "/LeftYoung.dat";

    SaveData _saveData;
    List<ISavable> _savableList = new List<ISavable>();

    public SaveData SaveData { get { return _saveData; } }
    protected override void Awake()
    {
        base.Awake();
        _savePath = Application.persistentDataPath;

        SceneChangeManager.Instance.OnEnterGame += RestoreAll;
    }

    public void Register(ISavable savable)
    {
        if (!_savableList.Contains(savable))
            _savableList.Add(savable);
    }
    public void Deregister(ISavable savable)
    {
        _savableList.Remove(savable);
    }

    public SaveData CaptureAll()
    {
        SaveData saveData = new SaveData();

        foreach(var savable in _savableList)
        {
            if (savable is PlayerController)
                saveData.playerSaveData = (PlayerSaveData)savable.CaptureData();
            else if (savable is InventoryManager)
                saveData.inventorySaveData = (InventorySaveData)savable.CaptureData();
            else if (savable is NpcControllerBase)
            {
                NpcProgressData npcData = (NpcProgressData)savable.CaptureData();
                saveData.npcProgressDict.Add(npcData.npcId, npcData);
            }
        }

        return saveData;
    }

    public void RestoreAll(SaveData saveData)
    {
        foreach(var savable in _savableList)
        {
            if (savable is PlayerController)
                savable.RestoreData(saveData.playerSaveData);
            else if (savable is InventoryManager)
                savable.RestoreData(saveData.inventorySaveData);
            else if( savable is NpcControllerBase)
                savable.RestoreData(saveData.npcProgressDict);
        }
    }
    public void RestoreAll()
    {
        Debug.Log("Restore Called");
        foreach (var savable in _savableList)
        {
            if (savable is PlayerController)
                savable.RestoreData(_saveData.playerSaveData);
            else if (savable is InventoryManager)
                savable.RestoreData(_saveData.inventorySaveData);
            else if (savable is NpcControllerBase)
                savable.RestoreData(_saveData.npcProgressDict);
        }
    }

    /* File */

    [ContextMenu("게임 저장")]
    public void SaveToFile()
    {
        _saveData = CaptureAll();

        BinaryFormatter formatter  = new BinaryFormatter();

        using(FileStream stream = new FileStream(_savePath+_fileName,FileMode.Create))
        {
            formatter.Serialize(stream, _saveData);
        }
     
        Debug.Log("저장 완료");
    }

    [ContextMenu("게임 불러오기")]
    public void LoadFromFile()
    {
       if(!File.Exists(_savePath+_fileName))
        {
            Debug.LogWarning("저장 파일이 없습니다.");
            return;
        }

       BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream stream = new FileStream(_savePath+_fileName,FileMode.Open))
        {
            _saveData = formatter.Deserialize(stream) as SaveData;

            //RestoreAll(_saveData);
            Debug.Log("파일 불러오기 완료");
        }
    }
}
