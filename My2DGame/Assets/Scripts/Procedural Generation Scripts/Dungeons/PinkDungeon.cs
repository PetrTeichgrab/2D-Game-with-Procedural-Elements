using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkDungeon : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    EnemyMushroomPink pinkMushroom;

    [SerializeField]
    Item pinkCrystal;

    [SerializeField]
    Item barel;

    [SerializeField]
    Item pinkStatue;

    Dungeon pinkDungeon;

    public void Create()
    {
        pinkDungeon = generator.PinkDungeon;
        CreateAndSetCharactersPositions();
    }

    private void CreateAndSetCharactersPositions()
    {
        for (int i = 0; i < 30; i++)
        {
            EnemyMushroomPink pinkMushroom = Instantiate(this.pinkMushroom, this.pinkMushroom.transform.position,
                this.pinkMushroom.transform.rotation);
            Item pinkCrystal = Instantiate(this.pinkCrystal);
            Item barel = Instantiate(this.barel);
            generator.setCharacterToRandomPosition(pinkMushroom, pinkDungeon, 5);
            generator.SetItemToEdgeOfRoom(barel, pinkDungeon);
            generator.SetItemToRandomPosition(pinkCrystal, pinkDungeon, 3);
        }
        for (int i = 0; i < 10; i++)
        {
            generator.SetLargeItemToRandomPosition(Instantiate(this.pinkStatue), pinkDungeon, 10, 7, 10);
        }
    }
}
