using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandSelectionState : BaseAbilityMenuState
{
    protected override void LoadMenu()
    {
        Debug.Log("selection LoadMenu ");
        owner.camRig.EndFollowTarget();
        if (menuOptions == null)
        {
            menuTitle = "Cmd";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Act");
            menuOptions.Add("Wait");
        }

        abilityMenuPanelController.MenuShow(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
    }
    protected override void Confirm()
    {
        Debug.Log("selection Confirm ");
        switch (abilityMenuPanelController.selection)
        {
            case 0: // 이동
                owner.ChangeState<MoveToTargetState>();
                break;
            case 1: // 행동
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2: // 대기
                owner.ChangeState<SelectUnitState>();
                break;
        }
    }
    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }
}