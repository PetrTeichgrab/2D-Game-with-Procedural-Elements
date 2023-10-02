using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkArguments",menuName = "ProceduralGeneration/RandomWalk")]
public class RandomWalkParameters : ScriptableObject
{
   public int iterations = 10;

    public int length = 20;

    public int corridorLength = 40;

    public int amountOfCorridors = 30;

    public bool startRandom = true;
}
