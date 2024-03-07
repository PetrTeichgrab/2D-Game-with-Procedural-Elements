using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    PinkDungeon pinkDungeon;

    [SerializeField]
    BlueDungeon blueDungeon;

    void Start()
    {
        generator.GenerateDungeons();
        pinkDungeon.Create();
        blueDungeon.Create();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
