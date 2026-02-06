using UnityEngine;
using UnityEngine.UI;

public class UI_ButtonPanel : MonoBehaviour
{
    public Button PotionButton;
    public Button GemButton;
    public Button SettingButton;

    public GameObject PotionPanel;

    public Button ExitButton;

    public void ShowPotionPanel()
    {
        PotionPanel.SetActive(true);
    }

    public void HidePanel()
    {
        PotionPanel.SetActive(false);
    }
}
