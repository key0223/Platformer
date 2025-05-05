using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    [SerializeField] SO_GridProperties[] _soGridProperties;

    protected override void Awake()
    {
        base.Awake();
    }

    public SO_GridProperties GetGridProperties(SceneName mapName)
    {
        foreach(SO_GridProperties mapData in _soGridProperties)
        {
            if(mapData.sceneName == mapName)
                return mapData;
        }
        return null;
    }
}
