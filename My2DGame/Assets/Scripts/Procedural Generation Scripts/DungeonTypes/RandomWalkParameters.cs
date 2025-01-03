using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkArguments",menuName = "ProceduralGeneration/RandomWalk")]
public class RandomWalkParameters : ScriptableObject
{
   public int iterations = 1000;

    public int minLength = 30;

    public int maxLength = 30;

    public int corridorLength = 5;

    public int amountOfCorridors = 30;

    public bool startRandom = false;
}
