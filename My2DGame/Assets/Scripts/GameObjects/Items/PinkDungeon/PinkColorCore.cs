using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkColorCore : ColorCore
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && character.isAlive)
            {
                character.AddHP(100);
                character.colorCores.Add(this);
            }
            gameObject.SetActive(false);
        }
    }
}
