using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;
    string[] Options1 = new string[] { "Act1", "Act2", "Act3" };
    string[] Options2 = new string[] { "Act1", "Act2", "Act3" };

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>(3);

        if (category == 0)
        {
            menuTitle = "Act1";
            SetOptions(Options1);
        }
        else
        {
            menuTitle = "Act2";
            SetOptions(Options2);
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    protected override void Confirm()
    {
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        owner.ChangeState<CommandSelectionState>();
    }

    protected override void Cancel()
    {
        owner.ChangeState<CategorySelectionState>();
    }

    void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}