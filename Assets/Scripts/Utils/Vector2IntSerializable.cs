using System;
using UnityEngine;

[Serializable]
public class Vector2IntSerializable 
{
    public int x;
    public int y;

    public Vector2IntSerializable(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2IntSerializable(Vector2Int vector2Int)
    {
        this.x = vector2Int.x;
        this.y = vector2Int.y;
    }
    
}
