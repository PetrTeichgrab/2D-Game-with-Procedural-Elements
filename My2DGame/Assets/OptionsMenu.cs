using UnityEngine;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown displayModeDropdown;

    void Start()
    {
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
}
