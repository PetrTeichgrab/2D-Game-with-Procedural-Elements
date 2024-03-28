using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleDungeon : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    //[SerializeField]
    //GreenBossSlime greenBossSlime;

    Dungeon purpleDungeon;

    public void Create()
    {
        purpleDungeon = generator.PurpleDungeon;
        CreateAndSetCharactersPositions();
    }

    private void CreateAndSetCharactersPositions()
    {
        //for (int i = 0; i < 15; i++)
        //{
        //    GreenSlime greenSlime = Instantiate(this.greenSlime, Vector2.zero, Quaternion.identity);
        //    greenSlime.attackCooldown = UnityEngine.Random.Range(greenSlime.attackMinCD, greenSlime.attackMaxCD);
        //    generator.SetToRandomPositionInRandomRoom(greenSlime.transform, greenDungeon, 1);
        //    generator.allEnemiesList.Add(greenSlime);
        //}
        //GreenBossSlime greenBossSlime = Instantiate(this.greenBossSlime, this.greenBossSlime.transform.position, this.greenBossSlime.transform.rotation);
        //generator.SetToRandomPositionInRandomRoom(greenBossSlime.transform, greenDungeon, 1);
        //generator.allEnemiesList.Add(greenBossSlime);
    }
}
