using System.Collections.Generic;
using UnityEngine;


public class MoveToTargetState : TurnState
{
    List<Tile> tiles;
    public override void Enter()
    {
        base.Enter();
        Movement m = turn.actor.GetComponent<Movement>();
        tiles = m.GetTilesInRange(board);
        board.SelectTiles(tiles);
    }
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
                owner.ChangeState<MoveSequenceState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
}