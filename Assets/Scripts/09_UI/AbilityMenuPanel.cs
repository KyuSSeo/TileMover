using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AbilityMenuPanel : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelObject;
    public List<TextMeshProUGUI> menuOptions;

    [Header("Settings")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    public void Show(string title, List<string> options)
    {
        panelObject.SetActive(true);
    }

    public void Hide()
    {
        panelObject.SetActive(false);
    }

    public void SetSelection(int index)
    {
        // 범위를 벗어나지 않게 처리
        if (menuOptions.Count == 0) return;

        for (int i = 0; i < menuOptions.Count; i++)
        {
            if (i == index)
            {
                menuOptions[i].color = selectedColor;
            }
            else
            {
                menuOptions[i].color = normalColor;
            }
        }
    }
}