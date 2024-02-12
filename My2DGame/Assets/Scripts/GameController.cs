using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    Player player;

    EnemyMushroomPink mushroomPink;
    void Start()
    {
        generator.GenerateDungeons();
        player = generator.Player;
        mushroomPink = generator.PinkMushroom;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMushroomAppearence();
    }

    public void CheckMushroomAppearence()
    {
        if (Vector2.Distance(player.transform.position, mushroomPink.transform.position) < 3)
        {
            mushroomPink.Appeard = true;
        }
    }
}
