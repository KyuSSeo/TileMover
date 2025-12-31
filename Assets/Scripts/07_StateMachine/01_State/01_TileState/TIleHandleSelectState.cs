using System.Collections.Generic;
using UnityEngine;

public class TileHandleSelectState : BaseAbilityMenuState
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
        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }
    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0: // Build
                SetTileAction(0);
                break;
            case 1: // Remove
                SetTileAction(1);
                break;
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
    void SetTileAction(int type)
    {
        owner.subCategory = type;
        owner.ChangeState<TileActionState>();
    }
}