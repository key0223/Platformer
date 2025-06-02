using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockController : MonoBehaviour
{
    [SerializeField] Tilemap _tileMap;

    int _blockSize = 32;
    Camera _mainCamera;

    Vector3 _cameraPos;
    Vector2Int _currentBlock;

    Dictionary<Vector2Int,Block> _blockDict = new Dictionary<Vector2Int,Block>();
    private void Awake()
    {
        _mainCamera = Camera.main;

        _cameraPos = _mainCamera.transform.position;
        _currentBlock = new Vector2Int(Mathf.FloorToInt(_cameraPos.x / _blockSize), Mathf.FloorToInt(_cameraPos.y / _blockSize));
    }

    void Start()
    {
        InitBlocks();
    }

    void InitBlocks()
    {
        BoundsInt mapbounds = _tileMap.cellBounds;
        int width = mapbounds.size.x;
        int height = mapbounds.size.y;

        int blockCountX = Mathf.CeilToInt((float)width / _blockSize);
        int blockCountY = Mathf.CeilToInt((float)height / _blockSize);

        for (int x = 0; x < blockCountX; x++)
        {
            for (int y = 0; y < blockCountY; y++)
            {
                Vector2Int blockId = new Vector2Int(x, y);
                BoundsInt bounds = new BoundsInt(mapbounds.xMin + x * _blockSize, mapbounds.yMin + y * _blockSize,0,_blockSize,_blockSize,1);

                _blockDict.Add(blockId, new Block(blockId, bounds));
            }
        }
    }

    void SetBlockAlpha(Vector2Int blockId, float alpha)
    {
        if (_blockDict.TryGetValue(blockId, out Block chunk))
        {
            foreach (Vector3Int pos in chunk.bounds.allPositionsWithin)
            {
                if (_tileMap.HasTile(pos))
                {
                    _tileMap.SetTileFlags(pos, TileFlags.None);
                    Color color = _tileMap.GetColor(pos);
                    color.a = alpha;
                    _tileMap.SetColor(pos, color);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SetBlockAlpha(new Vector2Int(0, 0), 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SetBlockAlpha(new Vector2Int(0, 0), 1f);
        }
    }
}
