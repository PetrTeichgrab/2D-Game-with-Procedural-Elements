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

    [SerializeField]
    GreenDungeon greenDungeon;

    [SerializeField]
    Player player;

    EnemyMushroomPinkBoss pinkDungeonBoss;

    void Start()
    {
        generateGameContent();
    }

    public void generateGameContent()
    {
        generator.GenerateDungeons();
        //blueDungeon.Create();
        //greenDungeon.Create();
        //pinkDungeon.Create();
    }

    // Update is called once per frame
    void Update()
    {
 
    }

}
