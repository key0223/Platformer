using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class TestStartButton : MonoBehaviour
{
    [SerializeField] Button _startButton;
    SceneName _startingScene = SceneName.Scene_Greenpath;
    Vector2 _startingPosition = new Vector2(2.5f, 4);

    void Awake()
    {
        _startButton.onClick.AddListener(() => GameStart());
    }

    void GameStart()
    {
        SaveLoadManager.Instance.LoadFromFile();
        SaveData saveData = SaveLoadManager.Instance.SaveData;

        if(saveData != null )
        {
            SceneChangeManager.ChangeSceneBySceneName(saveData.playerSaveData.currentScene);
        }
        else
        {
            SceneChangeManager.ChangeSceneBySceneName(_startingScene.ToString());
        }
    }
}
