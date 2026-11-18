using UnityEngine;

public static class VectorUtilities
{ 
    public static Vector2Int ToVector2Int(this Vector2 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.y);
    }

    public static Vector3 GetClosestAxis(this Vector3 axis)
    {
        var abs = new Vector3(
            Mathf.Abs(axis.x),
            Mathf.Abs(axis.y),
            Mathf.Abs(axis.z)
        );

        if (abs.x > abs.y && abs.x > abs.z)
            return Vector3.right * Mathf.Sign(axis.x);

        if (abs.y > abs.x && abs.y > abs.z)
            return Vector3.up * Mathf.Sign(axis.y);

        return Vector3.forward * Mathf.Sign(axis.z);
    }

    public static Vector3 GetSnappedRotation(this Vector3 rotation, float step)
    {
        //rotation = NormalizeVector(rotation);

        var snappedRotation = new Vector3(
            Mathf.Round(rotation.x / step) * step,
            Mathf.Round(rotation.y / step) * step,
            Mathf.Round(rotation.z / step) * step
        );

        return snappedRotation;
    }

    public static Vector3 NormalizeVector(this Vector3 vector)
    {
        return new Vector3(
            vector.x > 180f ? vector.x - 360f : vector.x,
            vector.y > 180f ? vector.y - 360f : vector.y,
            vector.z > 180f ? vector.z - 360f : vector.z);
    }
}
