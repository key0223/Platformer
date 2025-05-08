using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MapInformation : MonoBehaviour
{
    [SerializeField] MapName _mapName;


    void OnEnable()
    {
        MapManager.Instance.CurrentScene = _mapName;
    }


}
