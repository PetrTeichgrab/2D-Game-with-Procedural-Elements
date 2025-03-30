using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float dashSpeed;
    public float dashCD;
    public float movementSpeed;
    public float jumpForce;
    public int maxHP;
    public int money;
    public bool hasMovementSpeedSpell;
    public bool hasAttackSpeedSpell;
    public bool hasHealSpell;
    public bool hasTimeSlowSpell;
}
