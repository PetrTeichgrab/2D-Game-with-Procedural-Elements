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
            dashSpeed = player.dashSpeed,
            dashCD = player.dashCD,
            movementSpeed = player.movementSpeed,
            jumpForce = player.jumpForce,
            maxHP = player.maxHP,
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

            player.dashSpeed = data.dashSpeed;
            player.dashCD = data.dashCD;
            player.movementSpeed = data.movementSpeed;
            player.jumpForce = data.jumpForce;
            player.maxHP = data.maxHP;
        }
    }

    public static void SavePlayerSpell(CastSpell playerSpell)
    {
        PlayerSpellData data = new PlayerSpellData
        {
            spellSpeed = playerSpell.spellSpeed,
            cooldownTime = playerSpell.cooldownTime,
            damage = playerSpell.damage
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
                playerSpell.cooldownTime = data.cooldownTime;
                playerSpell.spellSpeed = data.spellSpeed;
                playerSpell.damage = data.damage;
            }
        }
    }
}

