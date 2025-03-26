using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    public Transform castPoint;
    public float spellSpeed = 2.5f;
    public int damage = 50;
    public Rigidbody2D rb;
    Vector2 mousePostion;
    public Camera cam;
    public GameObject hitEffect;
    public GameObject spellObject;

    [SerializeField]
    private Player player;

    public float cooldownTime = 0.2f;
    public float minCooldownTime = 0.1f;
    private float lastCastTime = -Mathf.Infinity;

    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        SaveSystem.LoadPlayerSpell(this);
    }

    void Update()
    {
        if (player.isDead) { 
            return; 
        }
        mousePostion = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mousePostion - (Vector2)rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        castPoint.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time >= lastCastTime + cooldownTime)
            {
                Cast();
                audioManager.PlaySFX(audioManager.playerSpell);
                lastCastTime = Time.time;
            }
            Debug.Log(player.movementSpeed);
            Debug.Log(damage);
        }
    }

    public void Cast()
    {
        if (spellObject == null)
        {
            Debug.LogWarning("spellObject je null!");
            return;
        }

        GameObject instance = Instantiate(spellObject, castPoint.position, castPoint.rotation);

        PlayerSpellBehavior behavior = instance.GetComponent<PlayerSpellBehavior>();
        if (behavior != null)
        {
            behavior.Initialize(this);
        }

        Rigidbody2D spellRb = instance.GetComponent<Rigidbody2D>();
        spellRb.AddForce(castPoint.up * spellSpeed, ForceMode2D.Impulse);
        Destroy(instance, 0.7f);
    }

    public void ReduceCooldown(float time)
    {
        if(cooldownTime-time <= minCooldownTime)
        {
            return;
        }
        cooldownTime -= time;
    }

    public void IncreaseDamage()
    {
        damage += 100;
    }
}

