using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Island", fileName = "Islands", order = 0)]
public class Islands : ScriptableObject
{
    public List<Transform> Center;
    public List<Transform> Side;
    public List<Transform> Angle;
}