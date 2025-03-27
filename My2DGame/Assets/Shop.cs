using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public int MOVEMENT_SPEED_SPELL_PRICE = 750;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button buyMovementSpeedSpellBtn;
    private Player player;

    private void Start()
    {
        player = Player.Instance;
        closeButton.onClick.AddListener(CloseShop);
        buyMovementSpeedSpellBtn.onClick.AddListener(BuyMovementSpeedSpell);
        if (player != null)
        {
            buyMovementSpeedSpellBtn.interactable = player.hasMovementSpeedSpell && player.money < MOVEMENT_SPEED_SPELL_PRICE;
        }
    }

    private void CloseShop()
    {
        Debug.Log("Shop zavøen");
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void BuyMovementSpeedSpell()
    {
        Debug.Log("Koupen spell na movement speed");
        player.hasMovementSpeedSpell = true;
    }
}

