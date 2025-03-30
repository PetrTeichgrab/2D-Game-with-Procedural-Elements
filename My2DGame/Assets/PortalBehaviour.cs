using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public DungeonBehaviour TargetDungeon;
    public Transform TargetPosition;
    public float cooldownTime = 1f;
    AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null || player.recentlyTeleported) return;

        if (TargetDungeon != null && TargetPosition != null)
        {
            collision.transform.position = TargetPosition.position;
            Debug.Log("Teleportováno do " + TargetDungeon.name);
            audioManager.PlaySFX(audioManager.teleport);
            StartCoroutine(StartTeleportCooldown(player));
        }
    }

    private IEnumerator StartTeleportCooldown(Player player)
    {
        player.recentlyTeleported = true;
        yield return new WaitForSeconds(cooldownTime);
        player.recentlyTeleported = false;
    }
}

