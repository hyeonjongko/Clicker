using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBar : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI minValueText;
    public Image fillBar;
    public TextMeshProUGUI maxValueText;

    [Header("Settings")]
    public Color barColor = new Color(0.8f, 0.5f, 0.2f); // 주황색 계열
    private const int MAX_STAT = 255;

    /// <summary>
    /// 스탯 바를 설정합니다
    /// </summary>
    public void SetStat(string statName, int value)
    {
        // 스탯 이름 포맷팅
        if (statNameText != null)
        {
            statNameText.text = FormatStatName(statName);
        }

        // 최소값 (실제 값)
        if (minValueText != null)
        {
            minValueText.text = value.ToString();
        }

        // 최대값 (255 고정)
        if (maxValueText != null)
        {
            maxValueText.text = MAX_STAT.ToString();
        }

        // 바 채우기 (0~1 사이 값)
        if (fillBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)value / MAX_STAT);
            fillBar.fillAmount = fillAmount;
            fillBar.color = barColor;
        }
    }

    /// <summary>
    /// 스탯 이름을 읽기 쉽게 포맷팅
    /// </summary>
    private string FormatStatName(string statName)
    {
        switch (statName.ToLower())
        {
            case "hp": return "Hp";
            case "attack": return "Attack";
            case "defense": return "Defense";
            case "special-attack": return "Special Attack";
            case "special-defense": return "Special Defense";
            case "speed": return "Speed";
            default: return statName;
        }
    }

    /// <summary>
    /// 바 색상을 동적으로 설정
    /// </summary>
    public void SetBarColor(Color color)
    {
        barColor = color;
        if (fillBar != null)
        {
            fillBar.color = color;
        }
    }
}
