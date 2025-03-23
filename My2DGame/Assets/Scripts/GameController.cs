using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    AudioManager audioManager;

    void Start()
    {
        generateGameContent();
        audioManager.PlayNextTrack();
    }

    public void generateGameContent()
    {
        generator.GenerateDungeons();
    }

    void Update()
    {
 
    }

}
