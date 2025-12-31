using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CategorySelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu()
    {
        Debug.Log("Category LoadMenu");
        if (menuOptions == null)
        {
            menuTitle = "Action";
            menuOptions = new List<string>(3);
            menuOptions.Add("TileControl");
            menuOptions.Add("Action1");
            menuOptions.Add("Action2");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    // 여기에서 행동의 대분류를 추가합니다.
    protected override void Confirm()
    {
        Debug.Log("Category Confirm");
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                owner.ChangeState<TileHandleSelectState>();
                break;
            case 1:
                SetCategory(0);
                break;
            case 2:
                SetCategory(1);
                break;
        }
    }

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        owner.ChangeState<CommandSelectionState>();
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }
}