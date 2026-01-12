using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionSelectionState : BaseAbilityMenuState
{
    public static int category;
    string[] Options1 = new string[] { "Act1", "Act2", "Act3"};
    string[] Options2 = new string[] { "Act1", "Act2"};

    protected override void LoadMenu()
    {
        Debug.Log("ActionSelectionState");
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

        abilityMenuPanelController.MenuShow(menuTitle, menuOptions);
    }

    // 세부 행동 추가
    protected override void Confirm()
    {
        Debug.Log("Action Confirm");
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                //
                break;
            case 1:
                // 
                break;
        }
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