using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDungeonGenerator
{
    public void GenerateDungeons();
    public void GenerateOneColorDungeon(IDungeon dungeon);
}
