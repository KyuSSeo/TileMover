using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHandleState : TurnState
{
    public override void Enter() 
    {
        Debug.Log("Tile Handle State Enter");
        base.Enter();
        Sequence();
    }

    private void Sequence()
    {
        TileObjectHandler handler = turn.actor.GetComponent<TileObjectHandler>();
        handler.BuildObstacle(owner.currentTile);
        turn.hasUnitActed = true;
        owner.ChangeState<CommandSelectionState>();
    }
}
