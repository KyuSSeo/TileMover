using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AbilityMenuPanel : MonoBehaviour
{
    [Header("Inspector Linked UI")]
    public GameObject panelObject; 
    public List<TextMeshProUGUI> menuOptions;
    public Image selectionIndicator; 

    public void Show(bool value)
    {
        panelObject.SetActive(value);
    }

    public void SetSelectionVisual(int index, Color selected, Color normal)
    {
        for (int i = 0; i < menuOptions.Count; i++)
        {
            menuOptions[i].color = (i == index) ? selected : normal;

            if (i == index && selectionIndicator != null)
            {
                selectionIndicator.transform.position = menuOptions[i].transform.position + new Vector3(-30f, 0, 0);
            }
        }
    }
}