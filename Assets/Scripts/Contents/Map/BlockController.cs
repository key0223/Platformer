using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockController : MonoBehaviour
{
    [SerializeField] Tilemap[] _minimaps; //  should be minimap tilemap for minimap UI _minimaps[0] is minimapLine;

    int _blockSize = 6;
    Camera _mainCamera;

    Vector3 _cameraPos;

    Vector2Int _prevBlock;
    Vector2Int _currentBlock;

    Dictionary<Vector2Int,Block> _blockDict = new Dictionary<Vector2Int,Block>();
    private void Awake()
    {
        _mainCamera = Camera.main;

        _cameraPos = _mainCamera.transform.position;
        UpdateBlock();
    }

    void Start()
    {
        InitBlocks();
    }

    void InitBlocks()
    {
        BoundsInt mapbounds = _minimaps[0].cellBounds;
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
        SetAllBlocksAlpha(0);
    }

    void SetBlockAlpha(Vector2Int blockId, float alpha)
    {
        if (_blockDict.TryGetValue(blockId, out Block chunk))
        {
            foreach (Tilemap tilemap in _minimaps)
            {
                foreach (Vector3Int pos in chunk.bounds.allPositionsWithin)
                {
                    if (tilemap.HasTile(pos))
                    {
                        tilemap.SetTileFlags(pos, TileFlags.None);
                        Color color = tilemap.GetColor(pos);
                        color.a = alpha;
                        tilemap.SetColor(pos, color);
                    }
                }
            }
        }
    }
    void SetAllBlocksAlpha(float alpha)
    {
        foreach (Vector2Int blockId in _blockDict.Keys)
        {
            SetBlockAlpha(blockId, alpha);
        }
    }

    void UpdateBlock()
    {
        _cameraPos = _mainCamera.transform.position;
        BoundsInt mapbounds = _minimaps[0].cellBounds;

        int blockX = Mathf.FloorToInt((_cameraPos.x - mapbounds.xMin) / _blockSize);
        int blockY = Mathf.FloorToInt((_cameraPos.y - mapbounds.yMin) / _blockSize);

        _currentBlock = new Vector2Int(blockX, blockY);
    }
    void Update()
    {
        UpdateBlock();

        if(_currentBlock != _prevBlock)
        {
            _prevBlock = _currentBlock;
        }

        if(_blockDict.TryGetValue(_currentBlock, out Block currentBlock) && !currentBlock.visited)
        {
            currentBlock.visited = true;
            SetBlockAlpha(_currentBlock, 1f);
        }
    }

    void OnDrawGizmos()
    {
        if (_blockDict == null || _blockDict.Count == 0)
            return;

        Gizmos.color = Color.yellow;
        foreach (var block in _blockDict.Values)
        {
            Vector3 min = _minimaps[0].CellToWorld(new Vector3Int(block.bounds.xMin, block.bounds.yMin, 0));
            Vector3 max = _minimaps[0].CellToWorld(new Vector3Int(block.bounds.xMax, block.bounds.yMax, 0));
            Vector3 size = max - min;

            Gizmos.DrawWireCube(min + size * 0.5f, size);
        }
    }
}
