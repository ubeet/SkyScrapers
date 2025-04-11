using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Create HeightInstance", fileName = "Height_x_x", order = 0)]
public class HeightInstance : ScriptableObject
{
    [NonSerialized] public int Usage;
    public GameObject instance;
    public Mesh mesh;
    public Renderer renderer;
    public bool LeftFront;
    public bool RightFront;
    public bool LeftRear;
    public bool RightRear;
    public bool Rear;

    public void Release()
    {
        Usage--;
    }

    public void Take()
    {
        Usage++;
    }
}