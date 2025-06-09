using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class TestStartButton : MonoBehaviour
{
    [SerializeField] Button _startButton;
    SceneName _startingScene = SceneName.Scene_Dirtmouth;
    Vector2 _startingPosition = new Vector2(-3.25f, 9.6f);

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
