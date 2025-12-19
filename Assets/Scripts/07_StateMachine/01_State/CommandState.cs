using UnityEngine;
using System.Collections.Generic;

public class CommandState : TurnState
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
        }
        else if (e.info == 1)
        {
            owner.ChangeState<SelectUnitState>();
        }
    }
}