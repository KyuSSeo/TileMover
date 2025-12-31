using System.Collections.Generic;
using UnityEngine;

public class TileeHandleSelectState : BaseAbilityMenuState
{

    string[] Options = new string[] { "Build", "Remove" };

    public override void Enter()
    {
        Debug.Log("Tile Select State Enter");
        base.Enter();
    }

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>();
        SetOptions(Options);
    }
    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                {
                    owner.ChangeState<TileBuildState>();
                    break;
                }
            case 1:
                {
                    owner.ChangeState<TileRemoveState>();
                    break;
                }
        }
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }

    protected override void Cancel()
    {
        Debug.Log("Cancel: CommandSelectionState로 돌아갑니다.");
        owner.ChangeState<CommandSelectionState>();
    }

    private void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }
}