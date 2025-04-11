using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Decorations", fileName = "Decorations", order = 0)]
public class Decorations : ScriptableObject
{
    public List<GameObject> decorations;
}