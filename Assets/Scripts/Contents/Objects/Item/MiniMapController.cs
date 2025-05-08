using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Define;

public class MiniMapController : MonoBehaviour
{
    PlayerTestCode _player;
    int _itemId = 0;
    int _checkRadius = 3;

    Dictionary<string, GridPropertyDetails> _gridPropertyDict = new Dictionary<string, GridPropertyDetails>();

    public int ItemID { get { return _itemId; } }
    public Dictionary<string, GridPropertyDetails> GridPropertyDict {  get { return _gridPropertyDict; } }
    void Start()
    {
        _player = FindObjectOfType<PlayerTestCode>();
        _player.OnPlayerMove += UpdateVisit;
    }

    public void Init(MiniMap miniMap)
    {
        _itemId = miniMap.ItemId;
        SO_GridProperties soGridProperties = MapManager.Instance.GetGridProperties(miniMap.MapName);

        foreach (GridCoordinate coordinate in soGridProperties.gridCoordinateList)
        {
            GridPropertyDetails gridPropertyDetails = new GridPropertyDetails();

            string key = "x" + coordinate.x + "y" + coordinate.y;

            if (_gridPropertyDict.ContainsKey(key))
                continue;

            gridPropertyDetails.gridX = coordinate.x;
            gridPropertyDetails.gridY = coordinate.y;

            gridPropertyDetails.visited = false;
            _gridPropertyDict.Add(key, gridPropertyDetails);
        }

        gameObject.SetActive(false);
    }

    public void UpdateVisit(int originX, int originY)
    {
        List<GridPropertyDetails> updateList = GetAffectedTiles(originX, originY);

        for (int i = 0; i < updateList.Count; i++)
        {
            updateList[i].visited = true;
        }
    }

    GridPropertyDetails GetGridPropertyDetails(int x, int y)
    {
        string key = "x" + x + "y" + y;

        GridPropertyDetails propertyDetails;

        if (_gridPropertyDict.TryGetValue(key, out propertyDetails))
            return propertyDetails;
        return null;
    }

    List<GridPropertyDetails> GetAffectedTiles(int originX, int originY)
    {
        List<GridPropertyDetails> result = new List<GridPropertyDetails>();

        for (int x = originX - _checkRadius; x < originX + _checkRadius; x++)
        {
            for (int y = originY - _checkRadius; y < originY + _checkRadius; y++)
            {
                int distance = Mathf.Abs(originX - x) + Mathf.Abs(originY - y);

                if (distance > _checkRadius) continue;

                GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(x, y);

                if (gridPropertyDetails != null && !gridPropertyDetails.visited)
                    result.Add(gridPropertyDetails);
            }
        }

        return result;
    }
}
