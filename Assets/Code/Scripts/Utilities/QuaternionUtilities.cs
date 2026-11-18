using UnityEngine;

public static class QuaternionUtilities
{
    public static Quaternion TransformToOtherSpace(Quaternion quaternion, Transform source, Transform target)
        => Quaternion.Inverse(target.rotation) * source.rotation * quaternion;

    public static Quaternion GetRotationRelative(this Quaternion quaternion, Transform otherSpace)
        => Quaternion.Inverse(otherSpace.rotation) * quaternion;

    public static Quaternion GetOriginalRotation(this Quaternion quaternion, Transform otherSpace)
        => otherSpace.rotation * quaternion;
}
