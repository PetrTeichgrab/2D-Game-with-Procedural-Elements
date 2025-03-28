using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public int MOVEMENT_SPEED_SPELL_PRICE = 750;
    public int ATTACK_SPEED_SPELL_PRICE = 1800;
    public int HEAL_SPELL_PRICE = 1600;
    public int TIMESLOW_SPELL_PRICE = 800;

    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyMovementSpeedSpellBtn;
    [SerializeField] private Button buyAttackSpeedSpellBtn;
    [SerializeField] private Button buyHealSpellBtn;
    [SerializeField] private Button buyTimeSlowSpellBtn;

    private Player player;

    private void Start()
    {
        player = Player.Instance;

        closeButton.onClick.AddListener(CloseShop);

        buyMovementSpeedSpellBtn.onClick.AddListener(BuyMovementSpeedSpell);
        buyAttackSpeedSpellBtn.onClick.AddListener(BuyAttackSpeedSpell);
        buyHealSpellBtn.onClick.AddListener(BuyHealSpell);
        buyTimeSlowSpellBtn.onClick.AddListener(BuyTimeSlowSpell);

        if (player != null)
        {
            buyMovementSpeedSpellBtn.interactable = !player.hasMovementSpeedSpell && player.money >= MOVEMENT_SPEED_SPELL_PRICE;
            buyAttackSpeedSpellBtn.interactable = !player.hasAttackSpeedSpell && player.money >= ATTACK_SPEED_SPELL_PRICE;
            buyHealSpellBtn.interactable = !player.hasHealSpell && player.money >= HEAL_SPELL_PRICE;
            buyTimeSlowSpellBtn.interactable = !player.hasTimeSlowSpell && player.money >= TIMESLOW_SPELL_PRICE;
        }
    }

    private void CloseShop()
    {
        Debug.Log("Shop zav�en");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void BuyMovementSpeedSpell()
    {
        if (player.money >= MOVEMENT_SPEED_SPELL_PRICE && !player.hasMovementSpeedSpell)
        {
            player.money -= MOVEMENT_SPEED_SPELL_PRICE;
            player.hasMovementSpeedSpell = true;
            Debug.Log("Koupen spell na movement speed");
            buyMovementSpeedSpellBtn.interactable = false;
        }
    }

    private void BuyAttackSpeedSpell()
    {
        if (player.money >= ATTACK_SPEED_SPELL_PRICE && !player.hasAttackSpeedSpell)
        {
            player.money -= ATTACK_SPEED_SPELL_PRICE;
            player.hasAttackSpeedSpell = true;
            Debug.Log("Koupen spell na attack speed");
            buyAttackSpeedSpellBtn.interactable = false;
        }
    }

    private void BuyHealSpell()
    {
        if (player.money >= HEAL_SPELL_PRICE && !player.hasHealSpell)
        {
            player.money -= HEAL_SPELL_PRICE;
            player.hasHealSpell = true;
            Debug.Log("Koupen heal spell");
            buyHealSpellBtn.interactable = false;
        }
    }

    private void BuyTimeSlowSpell()
    {
        if (player.money >= TIMESLOW_SPELL_PRICE && !player.hasTimeSlowSpell)
        {
            player.money -= TIMESLOW_SPELL_PRICE;
            player.hasTimeSlowSpell = true;
            Debug.Log("Koupen spell na zpomalen� �asu");
            buyTimeSlowSpellBtn.interactable = false;
        }
    }
}


