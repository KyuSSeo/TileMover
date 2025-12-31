using System.Collections.Generic;
using UnityEngine;


public class MoveToTargetState : TurnState
{
    List<Tile> tiles;
    public override void Enter()
    {
        Debug.Log("Enter MoveToTargetState");
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
        Debug.Log("Move OnFire");
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
            {
                owner.ChangeState<MoveSequenceState>();
            }

            /* A* Test
            Movement m = turn.actor.GetComponent<Movement>();
            Tile to = board.GetTile(pos);
            m.GetTilesInRange(owner.board, to);
            owner.ChangeState<MoveSequenceState>();
            */
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
}