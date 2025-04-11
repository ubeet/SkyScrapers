using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Create HeightContainer", fileName = "HeightContainer_", order = 0)]
public class HeightContainer : ScriptableObject
{
    public int Height;
    public List<HeightInstance> Instances;

    public HeightInstance Get()
    {
        HeightInstance min = Instances.FirstOrDefault();
        foreach (var instance in Instances.Where(instance => instance.Usage < min.Usage))
            min = instance;
        
        for (int i = Instances.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            (Instances[j], Instances[i]) = (Instances[i], Instances[j]);
        }
        
        return min;
    }
}