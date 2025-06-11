using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMapPanel : MonoBehaviour, ISavable
{
    [SerializeField] BlockController _currentMiniMap;

    GameObject _canvasGO;
    Grid _miniMapGrid;
    public GameObject CanvasGO { get { return _canvasGO; } }

    List<BlockController> _miniMaps = new List<BlockController>();

    void Awake()
    {
        _canvasGO = GetComponentInChildren<Canvas>().gameObject;
        _canvasGO.SetActive(false);

        _miniMapGrid = GetComponentInChildren<Grid>();

    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RegisterSave();
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DeregisterSave();
    }

    public void UpdateVisitedBlock()
    {
        _currentMiniMap.SetVisitedBlockAlpha();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(_currentMiniMap != null)
        {
            _currentMiniMap.gameObject.SetActive(false );
            _currentMiniMap = null;
        }
        string currentScene = scene.name;

        foreach (BlockController bc in _miniMaps)
        {
            if (bc.SceneName.ToString() != currentScene)
                continue;

            _currentMiniMap = bc;
            _currentMiniMap.gameObject.SetActive(true);
        }
    }
    public void AddMiniMap(Item miniMap)
    {
        ItemData itemData = DataManager.Instance.GetItemData(miniMap.ItemId);
        MapData mapData = itemData as MapData;
        if (mapData != null)
        {
            BlockController blockController = ResourceManager.Instance.Instantiate(mapData.miniMapPrefabPath).GetComponent<BlockController>();
            blockController.transform.SetParent(_miniMapGrid.transform, false);
            blockController.gameObject.SetActive(false);

            _miniMaps.Add(blockController);

            string currentScene = SceneManager.GetActiveScene().name;

            if(currentScene == mapData.sceneName.ToString())
            {
                _currentMiniMap = blockController;
                blockController.gameObject.SetActive(true);
            }
        }
    }
     
    #region Save
    public void RegisterSave()
    {
        SaveLoadManager.Instance.Register(this);
    }

    public void DeregisterSave()
    {
        SaveLoadManager.Instance.Deregister(this);
    }

    public object CaptureData()
    {
        Dictionary<string, MiniMapSaveData> allData = new Dictionary<string, MiniMapSaveData>();

        foreach(BlockController bc in _miniMaps)
        {
            allData[bc.SceneName.ToString()] = (MiniMapSaveData)bc.CaptureData();
        }

        return allData;
    }

    public void RestoreData(object loadedata)
    {
        Dictionary<string, MiniMapSaveData> allData = loadedata as Dictionary<string, MiniMapSaveData>;
        if (allData == null) return;

        // Create prefabs
        foreach(MiniMapSaveData data in allData.Values)
        {
            ItemData itemData = DataManager.Instance.GetItemData(data.itemId);
            MapData mapData = itemData as MapData;

            if (mapData != null)
            {
                BlockController blockController = ResourceManager.Instance.Instantiate(mapData.miniMapPrefabPath).GetComponent<BlockController>();
                blockController.transform.SetParent(_miniMapGrid.transform, false);
                blockController.gameObject.SetActive(false);

                _miniMaps.Add(blockController);

                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == mapData.sceneName.ToString())
                {
                    _currentMiniMap = blockController;
                    blockController.gameObject.SetActive(true);
                }
            }
        }
        foreach(BlockController bc in _miniMaps)
        {
            string sceneName = bc.SceneName.ToString();
            if (allData.ContainsKey(sceneName))
            {
                bc.RestoreData(allData[sceneName]);
            }
        }
    }
    #endregion

}
