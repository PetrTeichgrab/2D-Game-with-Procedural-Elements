using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColorCore : ColorCore
{
    private void Start()
    {
        color = DungeonColor.Blue;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && character.isAlive)
            {
                playPickUpSFX();
                character.movementSpeed += 1f;
                character.colorCores.Add(this);
            }
            gameObject.SetActive(false);
        }
    }
}
