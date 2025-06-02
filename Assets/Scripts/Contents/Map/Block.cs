using UnityEngine;
using UnityEngine.Tilemaps;

public class Block 
{
    public Vector2Int blockId;
    public BoundsInt bounds;
    public bool visited = false;

    public Block(Vector2Int blockId, BoundsInt bounds)
    {
        this.blockId = blockId;
        this.bounds = bounds;
    }
}
