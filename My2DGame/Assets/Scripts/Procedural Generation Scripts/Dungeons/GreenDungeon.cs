using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenDungeon : MonoBehaviour
{
    [SerializeField]
    DungeonGenerator generator;

    //[SerializeField]
    //GreenSlime greenSlime;

    //[SerializeField]
    //GreenBossSlime greenBossSlime;

    Dungeon greenDungeon;

    //public void Create()
    //{
    //    blueDungeon = generator.BlueDungeon;
    //    CreateAndSetCharactersPositions();
    //}

    //private void CreateAndSetCharactersPositions()
    //{
    //    for (int i = 0; i < 15; i++)
    //    {
    //        BlueSlime blueSlime = Instantiate(this.blueSlime, this.blueSlime.transform.position, this.blueSlime.transform.rotation);
    //        blueSlime.attackCooldown = UnityEngine.Random.Range(blueSlime.attackMinCD, blueSlime.attackMaxCD);
    //        generator.SetToRandomPositionInRandomRoom(blueSlime.transform, blueDungeon, 1);
    //        generator.allEnemiesList.Add(blueSlime);
    //    }
    //    BlueBossSlime blueBossSlime = Instantiate(this.blueBossSlime, this.blueBossSlime.transform.position, this.blueBossSlime.transform.rotation);
    //    generator.SetToRandomPositionInRandomRoom(blueBossSlime.transform, blueDungeon, 1);
    //    generator.allEnemiesList.Add(blueBossSlime);
    //}
}
