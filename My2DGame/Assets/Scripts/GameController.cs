using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    void Start()
    {
        generator.GenerateDungeons();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
