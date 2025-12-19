using UnityEngine;
using System.Collections.Generic;

public class CommandState : TurnState
{
    protected MenuController menuController;

    protected override void Awake()
    {
        base.Awake();
        menuController = FindFirstObjectByType<MenuController>();
    }

    public override void Enter()
    {
        base.Enter();
        menuController.Show();
    }

    public override void Exit()
    {
        base.Exit();
        menuController.Hide();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        menuController.OnMove(e.info);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            string choice = menuController.GetSelectionString();

            if (choice == "Move")
            {
                owner.ChangeState<MoveToTargetState>();
            }
            else if (choice == "Wait")
            {
                owner.turn.actor.
                owner.ChangeState<SelectUnitState>();
            }
        }
        else if (e.info == 1)
        {
            owner.ChangeState<SelectUnitState>();
        }
    }
}