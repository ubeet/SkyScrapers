using System.Drawing;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static class VectorExtensions
{
    public static SizeF ToSizeF(this Vector3 x) => new SizeF(x.x, x.y);
    
    public static SizeF ToSizeF(this Vector2 x) => new SizeF(x.x, x.y);
    
    public static Vector3 ConvertedTo3D(this Vector2 vector) => new Vector3(vector.x, 0, vector.y);

    public static Vector2 ConvertedTo2D(this Vector3 vector) => new Vector2(vector.x, vector.z);

    public static Vector3 Rotated(this Vector3 vector, Vector3 eulerAngles) =>
        vector.Rotated(eulerAngles.x, eulerAngles.y, eulerAngles.z);

    public static Vector3 Rotated(this Vector3 vector, float eulX, float eulY, float eulZ) =>
        Quaternion.Euler(eulX, eulY, eulZ) * vector;

    public static Vector2 Rotated(this Vector2 vector, float eul) =>
        Quaternion.Euler(0, 0, -eul) * vector;

    public static Vector3 ClampedMagnitude(this Vector3 vector, float clamp) =>
        Vector3.ClampMagnitude(vector, clamp);

    public static Vector2 ClampedMagnitude(this Vector2 vector, float clamp) =>
        Vector2.ClampMagnitude(vector, clamp);

    public static Vector3 MovedTowards(this Vector3 vector, Vector3 target, float maxDistanceDelta) =>
        Vector3.MoveTowards(vector, target, maxDistanceDelta);

    public static Vector2 MovedTowards(this Vector2 vector, Vector2 target, float maxDistanceDelta) =>
        Vector2.MoveTowards(vector, target, maxDistanceDelta);

    public static Vector3 ZeroY(this Vector3 vector) => new Vector3(vector.x, 0, vector.z);

    public static Vector3 ZeroZ(this Vector3 vector) => new Vector3(vector.x, vector.y);

    public static float AngleWith(this Vector3 thisVector, Vector3 vector) =>
        Vector3.Angle(thisVector, vector);

    public static float AngleWith(this Vector2 thisVector, Vector2 vector) =>
        Vector2.Angle(thisVector, vector);

    public static float SignedAngleWith(this Vector3 thisVector, Vector3 vector) =>
        thisVector.SignedAngleWith(vector, Vector3.up);

    public static float SignedAngleWith(this Vector3 thisVector, Vector3 vector, Vector3 axis) =>
        Vector3.SignedAngle(thisVector, vector, axis);

    public static float SignedAngleWith(this Vector2 thisVector, Vector2 vector) =>
        Vector2.SignedAngle(thisVector, vector);

    public static bool IsZero(this Vector3 vector) => vector == Vector3.zero;
    public static bool IsNotZero(this Vector3 vector) => vector != Vector3.zero;

    public static bool IsZero(this Vector2 vector) => vector == Vector2.zero;
    public static bool IsNotZero(this Vector2 vector) => vector != Vector2.zero;

    public static Vector3 DirectionTo(this Vector3 vector, Vector3 position) => position - vector;
    
    public static Vector2 DirectionTo(this Vector2 vector, Vector2 position) => position - vector;

    public static float DistanceTo(this Vector3 vector, Vector3 position) => Vector3.Distance(vector, position);

    public static float DistanceTo(this Vector2 vector, Vector3 position) => Vector2.Distance(vector, position);
    
    public static float DistanceXYTo(this Vector3 vector, Vector3 position) => Vector2.Distance(vector, position);
    
    public static Vector3 GraduatedAimError(this Vector3 vector ,float angledErrorPerShot, int shot)
    {
        if (shot == 0) return vector;
        var thisAngle = angledErrorPerShot * shot * 0.5f;
        return vector.Rotated(0, 0, Random.Range(-thisAngle, thisAngle));
    }
}