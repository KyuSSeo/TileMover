using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class AbilityMenuPanel : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelObject;
    public List<TextMeshProUGUI> menuOptions;
    public Image selectionCursor;

    [Header("Settings")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    public void Show(string title, List<string> options)
    {
        panelObject.SetActive(true);
        // 옵션에 맞춰 버튼을 활성화하거나 텍스트를 세팅
        // 간단히 고정된 메뉴 구현합니다.
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
                // 커서가 있다면 위치 이동
                if (selectionCursor != null)
                {
                    selectionCursor.rectTransform.position = menuOptions[i].rectTransform.position;
                }
            }
            else
            {
                menuOptions[i].color = normalColor;
            }
        }
    }
}