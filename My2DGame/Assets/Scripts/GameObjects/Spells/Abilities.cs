using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Ability 1")]
    public Image abilityImage1;
    public float cooldown1 = 10;
    bool isCooldown = false;
    public KeyCode ability1 = KeyCode.B;

    [SerializeField]
    Player player;

    private void Start()
    {
        abilityImage1.fillAmount = 0;
    }

    private void Update()
    {
        Ability1();
    }

    void Ability1()
    {
        if (Input.GetKey(ability1) && isCooldown == false){
            isCooldown = true;
            abilityImage1.fillAmount = 1;
            player.BoostMovementSpeedSpell(2, 5);
        }

        if (isCooldown) {
            abilityImage1.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

}
