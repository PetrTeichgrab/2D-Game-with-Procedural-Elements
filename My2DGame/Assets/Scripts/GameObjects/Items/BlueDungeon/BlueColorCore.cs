using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueColorCore : ColorCore
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            Player character = collision.gameObject.GetComponent<Player>();
            if (character != null && character.isAlive)
            {
                character.movementSpeed += 1.5f;
            }
            gameObject.SetActive(false);
        }
    }
}