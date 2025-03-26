using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LightGreenColorCore : ColorCore
{
    private void Start()
    {
        color = DungeonColor.LightGreen;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            Player character = collision.gameObject.GetComponent<Player>();
            CastSpell playerSpell = character.GetComponent<CastSpell>();
            if (character != null && playerSpell != null && character.isAlive)
            {
                playPickUpSFX();
                character.colorCores.Add(this);
                playerSpell.IncreaseDamage();
            }
            gameObject.SetActive(false);
        }
    }
}
