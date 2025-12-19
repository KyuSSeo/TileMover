using UnityEngine;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [SerializeField] AbilityMenuPanel view;

    public int selection { get; private set; }

    private List<string> options = new List<string> { "Move", "Wait" };

    private void Start()
    {
        if (view == null) view = GetComponent<AbilityMenuPanel>();
        view.Show(false);
    }

    public void Show()
    {
        view.Show(true);
        selection = 0;
        UpdateView();
    }

    public void Hide()
    {
        view.Show(false);
    }

    public void OnMove(Point delta)
    {
        if (delta.y > 0) selection--;
        if (delta.y < 0) selection++;

        if (selection < 0) selection = options.Count - 1;
        if (selection >= options.Count) selection = 0;

        UpdateView();
    }

    private void UpdateView()
    {
        view.SetSelectionVisual(selection, Color.yellow, Color.white);
    }

    public string GetSelectionString()
    {
        return options[selection];
    }
}
