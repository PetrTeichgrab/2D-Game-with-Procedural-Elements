using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private static string playerPath = Application.persistentDataPath + "/playerdata.json";
    private static string spellPath = Application.persistentDataPath + "/playerSpell.json";

    public static void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData
        {
            dashSpeed = player.dashSpeedPermanent,
            dashCD = player.dashCDPermanent,
            movementSpeed = player.movementSpeedPermanent,
            jumpForce = player.jumpForcePermanent,
            maxHP = player.maxHPpermanent,
            money = player.money,
            hasAttackSpeedSpell = player.hasAttackSpeedSpell,
            hasHealSpell = player.hasHealSpell,
            hasMovementSpeedSpell = player.hasMovementSpeedSpell,
            hasTimeSlowSpell = player.hasTimeSlowSpell
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(playerPath, json);
    }


    public static void LoadPlayer(Player player)
    {
        if (File.Exists(playerPath))
        {
            string json = File.ReadAllText(playerPath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            player.dashSpeedPermanent = data.dashSpeed;
            player.dashCDPermanent = data.dashCD;
            player.movementSpeedPermanent = data.movementSpeed;
            player.jumpForcePermanent = data.jumpForce;
            player.maxHPpermanent = data.maxHP;
            player.money = data.money;
            player.hasAttackSpeedSpell = data.hasAttackSpeedSpell;
            player.hasHealSpell = data.hasHealSpell;
            player.hasMovementSpeedSpell = data.hasMovementSpeedSpell;
            player.hasTimeSlowSpell = data.hasTimeSlowSpell;
        }
    }

    public static void SavePlayerSpell(CastSpell playerSpell)
    {
        PlayerSpellData data = new PlayerSpellData
        {
            spellSpeed = playerSpell.spellSpeedPermanent,
            cooldownTime = playerSpell.cooldownTimePermanent,
            damage = playerSpell.damagePermanent
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(spellPath, json);
    }

    public static void LoadPlayerSpell(CastSpell playerSpell)
    {
        if (File.Exists(spellPath))
        {
            string json = File.ReadAllText(spellPath);
            PlayerSpellData data = JsonUtility.FromJson<PlayerSpellData>(json);
            if (playerSpell != null)
            {
                playerSpell.cooldownTimePermanent = data.cooldownTime;
                playerSpell.spellSpeedPermanent = data.spellSpeed;
                playerSpell.damagePermanent = data.damage;
            }
        }
    }

    public static void ResetStats()
    {
        PlayerData playerData = new PlayerData
        {
            dashSpeed = Player.DEF_DASH_SPEED,
            dashCD = Player.DEF_DASH_CD,
            movementSpeed = Player.DEF_MOVEMENT_SPEED,
            jumpForce = Player.DEF_JUMP_FORCE,
            maxHP = Player.DEF_MAX_HP,
            money = Player.DEF_MONEY_AMOUNT,
            hasAttackSpeedSpell = false,
            hasHealSpell = false,
            hasMovementSpeedSpell = false,
            hasTimeSlowSpell = false
        };
        PlayerSpellData playerSpellData = new PlayerSpellData
        {
            spellSpeed = CastSpell.DEF_SPELL_SPEED,
            cooldownTime = CastSpell.DEF_COOLDOWN_TIME,
            damage = CastSpell.DEF_DAMAGE
        };

        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(playerPath, json);
        string jsonSpell = JsonUtility.ToJson(playerSpellData, true);
        File.WriteAllText(spellPath, jsonSpell);
    }
}

