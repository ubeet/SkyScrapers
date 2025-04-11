using System;
using UnityEngine;
using static UnityEngine.Random;
using Object = UnityEngine.Object;

public static class RandomExtension
{
    public static Vector2 RandomPositionBetweenTwoCircles(float range1, float range2)
    {
        Vector2 randomPoint = RandomPointOnCircle();
        randomPoint *= Range(range1, range2);
        return randomPoint;
    }

    public static Vector2 RandomPointOnCircle()
    {
        float angle = Range(0f, 360f);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    
    public static Vector2 RandomPointBetweenTwoRectangles(float x1, float y1, float x2, float y2)
    {
        var side = Range(0, 4);
        switch (side)
        {
            case 0:
                return new Vector2(Range(-x1, -x2), Range(-y2, y2));
            case 1:
                return new Vector2(Range(-x2, x2), Range(y1, y2));
            case 2:
                return new Vector2(Range(x1, x2), Range(-y2, y2));
            case 3:
                return new Vector2(Range(-x2, x2), Range(-y1, -y2));
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static Vector2 RandomPointOnRectangle(float xSize, float ySize)
    {
        var side = Range(0, 4);
        switch (side)
        {
            case 0:
                return new Vector2(-xSize, Range(-ySize, ySize));
            case 1:
                return new Vector2(Range(-xSize, xSize), ySize);
            case 2:
                return new Vector2(xSize, Range(-ySize, ySize));
            case 3:
                return new Vector2(Range(-xSize, xSize), -ySize);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public static Color RandomColor() => new Color(Range(0,1f),Range(0,1f),Range(0,1f));
    
    public static bool RandomBool() => Range(0, 2) == 1;

    public static T GetRandomObject<T>() where T : Object
    {
        var objects = GameObject.FindObjectsOfType<T>();
        return objects.GetRandomElement();
    }

    public static bool Roll(float chance) => chance >= Range(0f, 1f);

    public static Vector2 RandomPointInBounds(this Collider2D collider)
    {
        var bounds = collider.bounds;
        var point = new Vector2(Range(bounds.min.x, bounds.max.x), Range(bounds.min.y, bounds.max.y));
        if (collider.OverlapPoint(point) == false)
            point = collider.ClosestPoint(point);

        return point;
    }
}

