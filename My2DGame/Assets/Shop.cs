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

    public Abilities abilities;

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

    public void Update()
    {
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
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void BuyMovementSpeedSpell()
    {
        if (player.money >= MOVEMENT_SPEED_SPELL_PRICE && !player.hasMovementSpeedSpell)
        {
            player.money -= MOVEMENT_SPEED_SPELL_PRICE;
            player.hasMovementSpeedSpell = true;
            buyMovementSpeedSpellBtn.interactable = false;
            abilities.UpdateSpellsVisibility();
        }
    }

    private void BuyAttackSpeedSpell()
    {
        if (player.money >= ATTACK_SPEED_SPELL_PRICE && !player.hasAttackSpeedSpell)
        {
            player.money -= ATTACK_SPEED_SPELL_PRICE;
            player.hasAttackSpeedSpell = true;
            buyAttackSpeedSpellBtn.interactable = false;
            abilities.UpdateSpellsVisibility();
        }
    }

    private void BuyHealSpell()
    {
        if (player.money >= HEAL_SPELL_PRICE && !player.hasHealSpell)
        {
            player.money -= HEAL_SPELL_PRICE;
            player.hasHealSpell = true;
            buyHealSpellBtn.interactable = false;
            abilities.UpdateSpellsVisibility();
        }
    }

    private void BuyTimeSlowSpell()
    {
        if (player.money >= TIMESLOW_SPELL_PRICE && !player.hasTimeSlowSpell)
        {
            player.money -= TIMESLOW_SPELL_PRICE;
            player.hasTimeSlowSpell = true;
            buyTimeSlowSpellBtn.interactable = false;
            abilities.UpdateSpellsVisibility();
        }
    }
}


