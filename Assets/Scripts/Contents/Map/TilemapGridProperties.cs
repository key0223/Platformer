using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// For save coordinates of the minimap
/// </summary>

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
#if UNITY_EDITOR

    Tilemap _tilemap;
    [SerializeField] SO_GridProperties _gridProperties = null;

    void OnEnable()
    {
        if(!Application.IsPlaying(gameObject))
        {
            _tilemap = GetComponent<Tilemap>();
            if(_gridProperties !=null)
                _gridProperties.gridCoordinateList.Clear();
        }
    }

    void OnDisable()
    {
        if(!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();
            if(_gridProperties !=null)
                EditorUtility.SetDirty(_gridProperties); // 특정 객체의 변경 사항을 저장하도록 알립니다.
        }    
    }

    void UpdateGridProperties()
    {
        // 빈 공간 제거
        _tilemap.CompressBounds();

        if (!Application.IsPlaying(gameObject))
        {
            if (_gridProperties != null)
            {
                Vector3Int startCell = _tilemap.cellBounds.min;
                Vector3Int endCell = _tilemap.cellBounds.max;

                for (int x = startCell.x; x < endCell.x; x++)
                {
                    for (int y = startCell.y; y < endCell.y; y++)
                    {
                        TileBase tile = _tilemap.GetTile(new Vector3Int(x, y, 0));

                        if (tile != null)
                        {
                            _gridProperties.gridCoordinateList.Add(new GridCoordinate(x, y));
                        }
                    }
                }
            }
        }
    }

    void Update()
    {        
        if (!Application.IsPlaying(gameObject))
            Debug.Log("DISABLE PROPERTY TILEMAPS");
    }

#endif
}
