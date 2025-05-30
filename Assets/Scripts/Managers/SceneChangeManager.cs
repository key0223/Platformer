using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class SceneChangeManager : SingletonMonobehaviour<SceneChangeManager>
{
    static bool _loadFromDoor;

    public event Action OnEnterGame;
    public event Action OnSceneChanged;
    PlayerMovement _player;
    Collider2D _playerColl;
    Collider2D _doorColl;
    Vector3 _playerSpawnPosition;

    DoorToSpawnAt _doorToSpawnTo;

    SceneName _currentScene;

    public SceneName CurrentScene {  get { return _currentScene; } }
    protected override void Awake()
    {
        base.Awake();
    }

    public void OnEnterGameScene()
    {
        _player = GameObject.FindGameObjectWithTag(TAG_PLAYER).GetComponent<PlayerMovement>();
        _playerColl = _player.gameObject.GetComponent<Collider2D>();

        OnEnterGame?.Invoke();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region Scene change from door use 
    public static void ChangeSceneFromDoorUse(SceneField sceneToLoad, DoorToSpawnAt doorToSpawnAt = DoorToSpawnAt.None)
    {
        _loadFromDoor = true;
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(sceneToLoad, doorToSpawnAt));
    }

    IEnumerator FadeOutThenChangeScene(SceneField sceneToLoad, DoorToSpawnAt doorToSpawnAt = DoorToSpawnAt.None)
    {
        _player.SetMovementEnabled(true);
        SceneFadeManager.Instance.StartFadeOut();

        while (SceneFadeManager.Instance.IsFadingOut)
        {
            yield return null;
        }
        _doorToSpawnTo = doorToSpawnAt;
        OnSceneChanged?.Invoke();
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator ActivatePlayerMovementAfterFadeIn()
    {
        while (SceneFadeManager.Instance.IsFadingIn)
        {
            yield return null;
        }

        _player.SetMovementEnabled(false);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.Instance.StartFadeIn();

        if (_loadFromDoor)
        {
            StartCoroutine(ActivatePlayerMovementAfterFadeIn());
            FindDoor(_doorToSpawnTo);
            _player.gameObject.transform.position = _playerSpawnPosition;
            _loadFromDoor = false;
        }
    }

    void FindDoor(DoorToSpawnAt doorSpawnNumber)
    {
        DoorInteraction[] doors = FindObjectsOfType<DoorInteraction>();

        for (int i = 0; i < doors.Length; i++)
        {
            if (doors[i]._currentDoorPosition == doorSpawnNumber)
            {
                _doorColl = doors[i].gameObject.GetComponent<Collider2D>();

                CalculateSpawnPosition();
                return;
            }
        }
    }

    void CalculateSpawnPosition()
    {
        float colliderHeight = _playerColl.bounds.extents.y;
        _playerSpawnPosition = _doorColl.transform.position - new Vector3(0f, colliderHeight, 0f);
    }
    #endregion

    #region Scene change by scene name
    public static void ChangeSceneBySceneName(string sceneName)
    {
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(sceneName));
    }

    IEnumerator FadeOutThenChangeScene(string sceneName)
    {
        SceneFadeManager.Instance.StartFadeOut();

        while (SceneFadeManager.Instance.IsFadingOut)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);

        OnSceneLoaded();
    }

    void OnSceneLoaded()
    {
        SceneFadeManager.Instance.StartFadeIn();

        GameObject contentsObject = Instantiate(Resources.Load<GameObject>("Prefabs/GAME CONTENTS OBJECT"));
        OnEnterGameScene();
        //_player.SetMovementEnabled(true);

        DontDestroyOnLoad(contentsObject);
    }
    #endregion

}
