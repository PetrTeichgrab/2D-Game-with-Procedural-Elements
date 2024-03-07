using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkDungeon : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    EnemyMushroomPink pinkMushroom;

    Dungeon pinkDungeon;

    public void Create()
    {
        pinkDungeon = generator.PinkDungeon;
        CreateAndSetCharactersPositions();
    }

    private void CreateAndSetCharactersPositions()
    {
        Debug.Log("got here");
        for (int i = 0; i < 30; i++)
        {
            EnemyMushroomPink pinkMushroom = Instantiate(this.pinkMushroom, this.pinkMushroom.transform.position,
                this.pinkMushroom.transform.rotation);
            generator.allEnemiesList.Add(pinkMushroom);
            generator.SetToRandomPositionInRandomRoom(pinkMushroom.transform, pinkDungeon, 5);
        }
    }
}
