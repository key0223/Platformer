using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class MiniMapUI : MonoBehaviour
{
    MiniMapController _miniMapController;

    [SerializeField] Tilemap _mapLine;
    [SerializeField] Tilemap _mapBackground;

    void Start()
    {
        _miniMapController = GetComponent<MiniMapController>();

        InitTiles();
    }

    void InitTiles()
    {
        List<GridPropertyDetails> detailList = _miniMapController.GridPropertyDict.Values.ToList();

        foreach(GridPropertyDetails detail in detailList)
        {
            Vector3Int tilePos = new Vector3Int(detail.gridX, detail.gridY);

            Color originColor = _mapLine.GetColor(tilePos);
            Color newColor = new Color(originColor.r, originColor.g, originColor.b, 0f);

            
            if(_mapLine.HasTile(tilePos))
            {
                _mapLine.SetTileFlags(tilePos, TileFlags.None);
                _mapLine.SetColor(tilePos, newColor);
            }

            if (_mapBackground.HasTile(tilePos))
            {
                _mapBackground.SetTileFlags(tilePos, TileFlags.None);
                _mapBackground.SetColor(tilePos, newColor);
            }
        }

    }
   
    public void UpdateMiniMap()
    {
        List<GridPropertyDetails> detailList = _miniMapController.GridPropertyDict.Where(detail =>detail.Value.visited==true).Select(detail=>detail.Value).ToList();

        foreach(GridPropertyDetails detail in detailList)
        {
            Vector3Int tilePos = new Vector3Int(detail.gridX, detail.gridY);

            Color originColor = _mapLine.GetColor(tilePos);

            Color newLineColor = new Color(originColor.r, originColor.g, originColor.b,1f);
            Color newBackColor = new Color(originColor.r, originColor.g, originColor.b, 0.7f);


            if (_mapLine.HasTile(tilePos))
                _mapLine.SetColor(tilePos, newLineColor);

            if(_mapBackground.HasTile(tilePos))
                _mapBackground.SetColor(tilePos, newBackColor);
        }

        Debug.Log("MiniMap updated");
    }

}
