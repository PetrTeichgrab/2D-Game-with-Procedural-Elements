using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown displayModeDropdown;

    [SerializeField] private Toggle devToolsToggle;
    [SerializeField] private Image toggleBackground;

    void Start()
    {
        bool enabled = PlayerPrefs.GetInt("DevTools", 0) == 1;
        devToolsToggle.isOn = enabled;
        devToolsToggle.onValueChanged.AddListener(SetDevTools);
        UpdateToggleColor(enabled);

        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        displayModeDropdown.value = Screen.fullScreen ? 0 : 1;

        if (displayModeDropdown.captionText != null)
            displayModeDropdown.captionText.fontSize = 28;

        if (displayModeDropdown.itemText != null)
            displayModeDropdown.itemText.fontSize = 28;

        Transform item = displayModeDropdown.template.Find("Viewport/Content/Item");
        if (item != null)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            if (itemRect != null)
                itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, 40);
        }
    }

    public void SetDisplayMode(int index)
    {
        if (index == 1)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    public void SetDevTools(bool isEnabled)
    {
        PlayerPrefs.SetInt("DevTools", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
        UpdateToggleColor(isEnabled);
    }

    private void UpdateToggleColor(bool isEnabled)
    {
        if (toggleBackground != null)
            toggleBackground.color = isEnabled ? Color.green : Color.white;
    }
}
