using UnityEngine;

public static class VectorUtilities
{ 
    public static Vector2Int ToVector2Int(this Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }
}
