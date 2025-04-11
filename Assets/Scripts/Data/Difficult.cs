using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Difficulties")]
public class Difficult : ScriptableObject
{
    public List<Stage> Stages = new List<Stage>();
}