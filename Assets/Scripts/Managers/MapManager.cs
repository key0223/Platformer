using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static Define;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    [SerializeField] SO_GridProperties[] _soGridProperties;

    MapName _currentScene;
    MiniMapController _currentMiniMap;
    Dictionary<MapName,MiniMapController> _purchasedMiniMaps = new Dictionary<MapName, MiniMapController>();

    public MapName CurrentScene
    { 
        get { return _currentScene; } 
        set 
        {
            if (_currentScene == value) return;
            _currentScene = value;
            UpdateCurrentMiniMap(value);

        } 
    }
    public MiniMapController CurrentMiniMap { get { return _currentMiniMap; } }
    public Dictionary<MapName,MiniMapController> PurchasedMiniMaps { get {  return _purchasedMiniMaps; } }


    protected override void Awake()
    {
        base.Awake();
    }

    void UpdateCurrentMiniMap(MapName mapName)
    {
        if (_purchasedMiniMaps.ContainsKey(mapName))
        {
            _currentMiniMap = _purchasedMiniMaps[mapName];

            
        }
    }

    public SO_GridProperties GetGridProperties(MapName mapName)
    {
        foreach(SO_GridProperties mapData in _soGridProperties)
        {
            if(mapData.mapName == mapName)
                return mapData;
        }
        return null;
    }

    public void OnMiniMapPurchased(Item item)
    {
        MiniMap miniMap = item as MiniMap;

        Transform controllerParent = UIManager.Instance.PopupPanel.MiniMapPanel.MiniMapParent.transform;
        MiniMapController controller = ResourceManager.Instance.Instantiate(miniMap.PrefabPath, controllerParent).GetComponent<MiniMapController>();
        controller.Init(miniMap);
        _purchasedMiniMaps.Add(miniMap.MapName, controller);
    }
}
