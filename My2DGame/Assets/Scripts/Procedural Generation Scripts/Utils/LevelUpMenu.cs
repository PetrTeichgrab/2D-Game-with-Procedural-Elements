using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private CastSpell spellCasting;

    [SerializeField]
    private Button button1;
    [SerializeField]
    private Button button2;
    [SerializeField]
    private Button button3;

    [SerializeField]
    Sprite pinkButtonSprite;

    [SerializeField]
    Sprite blueButtonSprite;

    [SerializeField]
    Sprite greenButtonSprite;

    [SerializeField]
    Sprite purpleButtonSprite;

    private List<Button> buttons;

    private static System.Random random = new System.Random();

    private bool hasDisplayedLevelUp = false;


    void Start()
    {
        buttons = new List<Button> { button1, button2, button3 };
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player.isDead && !hasDisplayedLevelUp)
        {
            hasDisplayedLevelUp = true;
            StartCoroutine(ShowLevelUpButtonsAfterDelay(4f));
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            hasDisplayedLevelUp = true;
            StartCoroutine(ShowLevelUpButtonsAfterDelay(4f));
        }
    }

    private IEnumerator ShowLevelUpButtonsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GenerateLevelUpButtons();
    }

    private void HideLevelUpButtons()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    void GenerateLevelUpButtons()
    {
        List<DungeonColor> selectedColors = GetRandomColors(3);

        for (int i = 0; i < buttons.Count; i++)
        {
            Button currentButton = buttons[i];
            Image buttonImage = currentButton.GetComponent<Image>();

            if (i < selectedColors.Count)
            {
                DungeonColor colorCoreColour = selectedColors[i];
                var sprite = convertDungeonColorToSprite(colorCoreColour);
                if (sprite != null)
                {
                    buttonImage.sprite = sprite;
                }

                if (PlayerHasColorCore(colorCoreColour))
                {
                    currentButton.interactable = true;
                    currentButton.onClick.RemoveAllListeners();
                    currentButton.onClick.AddListener(() => ApplyUpgrade(GetColorCoreFromColor(colorCoreColour)));
                }
                else
                {
                    currentButton.interactable = false;
                }
                currentButton.gameObject.SetActive(true);
            }
            else
            {
                currentButton.gameObject.SetActive(false);
            }
        }
    }

    bool PlayerHasColorCore(DungeonColor color)
    {
        return player.colorCores.Exists(cc => cc.color == color);
    }

    ColorCore GetColorCoreFromColor(DungeonColor color)
    {
        return player.colorCores.Find(cc => cc.color == color);
    }


    List<DungeonColor> GetRandomColors(int count)
    {
        List<DungeonColor> availableColors = new List<DungeonColor>
        {
            DungeonColor.Pink,
            DungeonColor.Blue,
            DungeonColor.Green,
            DungeonColor.Purple,
        };
        List<DungeonColor> randomColors = new List<DungeonColor>();

        for (int i = 0; i < count && availableColors.Count > 0; i++)
        {
            int randomIndex = random.Next(0, availableColors.Count);
            randomColors.Add(availableColors[randomIndex]);
            availableColors.RemoveAt(randomIndex);
        }

        return randomColors;
    }

    void ApplyUpgrade(ColorCore colorCore)
    {
        Debug.Log($"Applied upgrade for {colorCore.color}");
        switch (colorCore.color)
        {
            case DungeonColor.Pink:
                player.maxHP += 10;
                player.currentHP = player.maxHP;
                break;
            case DungeonColor.Blue:
                player.movementSpeed += 0.6f;
                break;
            case DungeonColor.Green:
                spellCasting.ReduceCooldown(0.1f);
                break;
            case DungeonColor.Purple:
                player.ReduceDashCD(0.2f);
                break;

        }
        SceneManager.LoadScene("Endscene");
    }

    Color convertDungeonColorToColor(DungeonColor color)
    {
        switch (color)
        {
            case DungeonColor.Pink:
                return new Color(1f, 0.75f, 0.8f);
            case DungeonColor.Blue:
                return Color.blue;
            case DungeonColor.Green:
                return Color.green;
            case DungeonColor.Purple:
                return new Color(0.5f, 0f, 0.5f);
            case DungeonColor.Yellow:
                return Color.yellow; 
            case DungeonColor.Red:
                return Color.red; 
            case DungeonColor.Black:
                return Color.black; 
            default:
                return Color.white;
        }
    }

    Sprite convertDungeonColorToSprite(DungeonColor color)
    {
        switch (color)
        {
            case DungeonColor.Pink:
                return pinkButtonSprite;
            case DungeonColor.Blue:
                return blueButtonSprite;
            case DungeonColor.Green:
                return greenButtonSprite;
            case DungeonColor.Purple:
                return purpleButtonSprite;
            default:
                return null;
        }
    }

}
