using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenColorCore : ColorCore
{
    private void Start()
    {
        color = DungeonColor.Green;
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
                playerSpell.ReduceCooldown(0.8f);
                character.colorCores.Add(this);
            }
            gameObject.SetActive(false);
        }
    }
}
