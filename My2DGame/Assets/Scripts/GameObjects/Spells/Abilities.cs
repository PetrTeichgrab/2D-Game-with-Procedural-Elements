using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Ability 1 - Movement Speed")]
    public Image abilityImage1;
    public float cooldown1 = 25;
    private bool isCooldown1 = false;
    public KeyCode ability1 = KeyCode.Alpha1;

    [Header("Ability 2 - Attack Speed")]
    public Image abilityImage2;
    public float cooldown2 = 35;
    private bool isCooldown2 = false;
    public KeyCode ability2 = KeyCode.Alpha3;

    [Header("Ability 3 - Heal")]
    public Image abilityImage3;
    public float cooldown3 = 45;
    private bool isCooldown3 = false;
    public KeyCode ability3 = KeyCode.Alpha2;

    [Header("Ability 4 - Time Slow")]
    public Image abilityImage4;
    public float cooldown4 = 15;
    private bool isCooldown4 = false;
    public KeyCode ability4 = KeyCode.Alpha4;

    [Header("Ability 5 - Eagle eye")]
    public Image abilityImage5;
    public float cooldown5 = 20;
    private bool isCooldown5 = false;
    public KeyCode ability5 = KeyCode.Alpha5;

    [SerializeField] private Player player;

    private AudioManager audioManager;

    private void Start()
    {
        UpdateSpellsVisibility();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (!player.cantUseSpells)
        {
            Ability1();
            Ability2();
            Ability3();
            Ability4();
            Ability5();
        }
    }

    public void UpdateSpellsVisibility()
    {
        abilityImage1.fillAmount = player.hasMovementSpeedSpell ? 0 : 1;
        abilityImage2.fillAmount = player.hasAttackSpeedSpell ? 0 : 1;
        abilityImage3.fillAmount = player.hasHealSpell ? 0 : 1;
        abilityImage4.fillAmount = player.hasTimeSlowSpell ? 0 : 1;
        abilityImage5.fillAmount = 0;
    }

    private void Ability1()
    {
        if (player.hasMovementSpeedSpell && Input.GetKeyDown(ability1) && !isCooldown1)
        {
            isCooldown1 = true;
            abilityImage1.fillAmount = 1;
            player.BoostMovementSpeedSpell(2, 5);
        }

        if (isCooldown1)
        {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown1 = false;
            }
        }
    }

    private void Ability2()
    {
        if (player.hasAttackSpeedSpell && Input.GetKeyDown(ability2) && !isCooldown2)
        {
            isCooldown2 = true;
            abilityImage2.fillAmount = 1;
            player.BoostAttackSpeedSpell(1, 3);
        }

        if (isCooldown2)
        {
            abilityImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (abilityImage2.fillAmount <= 0)
            {
                abilityImage2.fillAmount = 0;
                isCooldown2 = false;
            }
        }

    }

    private void Ability3()
    {
        if (player.hasHealSpell && Input.GetKeyDown(ability3) && !isCooldown3)
        {
            isCooldown3 = true;
            abilityImage3.fillAmount = 1;
            player.HealSpell();
        }

        if (isCooldown3)
        {
            abilityImage3.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (abilityImage3.fillAmount <= 0)
            {
                abilityImage3.fillAmount = 0;
                isCooldown3 = false;
            }
        }
    }

    private void Ability4()
    {
        if (player.hasTimeSlowSpell && Input.GetKeyDown(ability4) && !isCooldown4)
        {
            isCooldown4 = true;
            abilityImage4.fillAmount = 1;
            player.TimeSlowSpell(1.5f);
        }

        if (isCooldown4)
        {
            abilityImage4.fillAmount -= 1 / cooldown4 * Time.deltaTime;

            if (abilityImage4.fillAmount <= 0)
            {
                abilityImage4.fillAmount = 0;
                isCooldown4 = false;
            }
        }
    }

    private void Ability5()
    {
        if (Input.GetKeyDown(ability5) && !isCooldown5)
        {
            isCooldown5 = true;
            abilityImage5.fillAmount = 1;
            audioManager.PlaySFX(audioManager.eagleEyeSpell);
            ActivateCameraZoomSpell();
        }

        if (isCooldown5)
        {
            abilityImage5.fillAmount -= 1 / cooldown5 * Time.deltaTime;

            if (abilityImage5.fillAmount <= 0)
            {
                abilityImage5.fillAmount = 0;
                isCooldown5 = false;
            }
        }

    }

    public void ActivateCameraZoomSpell()
    {
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.ActivateZoomOut();
        }
    }

}

