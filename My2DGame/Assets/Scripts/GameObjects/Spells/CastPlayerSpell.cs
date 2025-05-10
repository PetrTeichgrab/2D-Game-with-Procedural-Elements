using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    public static int DEF_DAMAGE = 15;
    public static float DEF_SPELL_SPEED = 5;
    public static float DEF_COOLDOWN_TIME = 1.2f;

    public Transform castPoint;
    public float spellSpeedPermanent = DEF_SPELL_SPEED;
    public float spellSpeed = 0;
    public int damagePermanent = DEF_DAMAGE;
    public int damage = 0;
    public float cooldownTimePermanent = DEF_COOLDOWN_TIME;
    public float cooldownTime = 1f;
    public Rigidbody2D rb;
    Vector2 mousePostion;
    public Camera cam;
    public GameObject hitEffect;
    public GameObject spellObject;

    [SerializeField]
    private Player player;

    public float minCooldownTime = 0.5f;
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

        if (Input.GetButton("Fire1"))
        {
            if (Time.time >= lastCastTime + cooldownTimePermanent + cooldownTime)
            {
                Cast();
                audioManager.PlaySFX(audioManager.playerBasicSpell);
                lastCastTime = Time.time;
            }
        }


        if (Input.GetKeyDown(KeyCode.Delete) && PlayerPrefs.GetInt("DevTools", 0) == 1)
        {
            resetStats();
        }
    }

    public void resetStats()
    {
        spellSpeedPermanent = DEF_SPELL_SPEED;
        spellSpeed = 0;
        damagePermanent = DEF_DAMAGE;
        damage = 0;
        cooldownTimePermanent = DEF_COOLDOWN_TIME;
        cooldownTime = 1f;
    }

    public void Cast()
    {
        if (spellObject == null)
        {
            return;
        }

        GameObject instance = Instantiate(spellObject, castPoint.position, castPoint.rotation);

        PlayerSpellBehavior behavior = instance.GetComponent<PlayerSpellBehavior>();
        if (behavior != null)
        {
            behavior.Initialize(this);
        }

        Rigidbody2D spellRb = instance.GetComponent<Rigidbody2D>();
        spellRb.AddForce(castPoint.up * (spellSpeedPermanent + spellSpeed), ForceMode2D.Impulse);
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

    public void ReduceCooldownPermanent(float time)
    {
        if (cooldownTimePermanent - time <= minCooldownTime)
        {
            return;
        }
        cooldownTimePermanent -= time;
    }

    public void IncreaseDamagePermanent()
    {
        damagePermanent += 5;
    }
    public void IncreaseDamage()
    {
        damage += 10;
    }
}

