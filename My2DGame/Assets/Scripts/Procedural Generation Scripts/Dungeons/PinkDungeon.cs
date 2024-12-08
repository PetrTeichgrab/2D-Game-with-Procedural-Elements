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

    [SerializeField]
    Item pinkRock1small;

    [SerializeField]
    Item pinkRock1med;

    [SerializeField]
    Item pinkRock2large;

    Dungeon pinkDungeon;

    public void Create()
    {
        pinkDungeon = generator.PinkDungeon;
        CreateAndSetCharactersPositions();
    }

    private void CreateAndSetCharactersPositions()
    {
        //generator.SetLargeItemToRoomCenter(Instantiate(this.pinkRock2large), pinkDungeon, 4, 4);
        //for (int i = 0; i < 30; i++)
        //{
        //    EnemyMushroomPink pinkMushroom = Instantiate(this.pinkMushroom, this.pinkMushroom.transform.position,
        //        this.pinkMushroom.transform.rotation);
        //    Item pinkCrystal = Instantiate(this.pinkCrystal);
        //    Item pinkRock1sm = Instantiate(this.pinkRock1small);
        //    Item pinkRock1med = Instantiate(this.pinkRock1med);
        //    generator.setCharacterToRandomPosition(pinkMushroom, pinkDungeon, 5);
        //    generator.SetItemToEdgeOfRoom(pinkRock1sm, pinkDungeon);
        //    generator.SetItemToRandomPosition(pinkCrystal, pinkDungeon, 3);
        //    generator.SetItemToRandomPosition(pinkRock1sm, pinkDungeon, 3);
        //    generator.SetItemToRandomPosition(pinkRock1med, pinkDungeon, 3);

        //}
    }
}
