using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public static class ListExtensions
{
    public static T GetRandomElement<T>(this IList<T> list)
    {
        var count = list.Count;
        if (count == 0) return default(T);
        if (count == 1) return list[0];
        
        int index = (int) Math.Floor(Random.Range(0, count * 10f) / 10f);
        index = Math.Clamp(index, 0, count - 1);
        return list[index];
    }

    public static List<T> Clone<T>(this List<T> list)
    {
        var clone = new List<T>();
        clone.AddRange(list);
        return clone;
    }

    public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
    {
        var count = enumerable.Count();
        if (count == 0) return default;
        var random = Random.Range(0, count);
        if (enumerable.ElementAt(random) == null) return enumerable.ElementAt(0);
        return enumerable.ElementAt(random);
    }
    
    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = new System.Random().Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static bool IsValid<T>(this List<T> list) => list is {Count: >0};


    public static bool RemoveIfContains<T>(this List<T> list, T item)
    {
        if(list == null)  return false;
        if (!list.Contains(item)) return false;
        list.Remove(item);
        return true;
    }

    public static bool IsEmptyOrInvalid<T>(this List<T> list) => list is null or {Count: 0};
    
    public static bool IsValid<T>(this IList<T> list) => list is {Count: >0};
    public static bool IsEmptyOrInvalid<T>(this IList<T> list) => list is null or {Count: 0};

    public static bool IsIndexInBounds<T>(this IReadOnlyList<T> list, int index) => (index >= 0) && (index < list.Count);
}

