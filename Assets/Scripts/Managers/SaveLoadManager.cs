using System.Collections;
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
    protected override void Awake()
    {
        base.Awake();
        _savePath = Application.persistentDataPath;
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
        }

        return saveData;
    }

    public void RestoreAll(SaveData saveData)
    {
        foreach(var savable in _savableList)
        {
            if (savable is PlayerSaveData)
                savable.RestoreData(saveData.playerSaveData);
        }
    }

    /* File */

    [ContextMenu("���� ����")]
    public void SaveToFile()
    {
        _saveData = CaptureAll();

        BinaryFormatter formatter  = new BinaryFormatter();

        using(FileStream stream = new FileStream(_savePath+_fileName,FileMode.Create))
        {
            formatter.Serialize(stream, _saveData);
        }
     
        Debug.Log("���� �Ϸ�");
    }

    [ContextMenu("���� �ҷ�����")]
    public void LoadFromFile()
    {
       if(!File.Exists(_savePath+_fileName))
        {
            Debug.LogWarning("���� ������ �����ϴ�.");
            return;
        }

       BinaryFormatter formatter = new BinaryFormatter();
        using(FileStream stream = new FileStream(_savePath+_fileName,FileMode.Open))
        {
            _saveData = formatter.Deserialize(stream) as SaveData;

            RestoreAll(_saveData);
            Debug.Log("���� �ҷ����� �Ϸ�");
        }
    }
}
