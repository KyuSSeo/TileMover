using System.Collections.Generic;
using UnityEngine;

public class TIleHandleSelectState : TurnState
{
    List<Tile> tiles;

    public override void Enter()
    {
        Debug.Log("Tile Select State Enter");
        base.Enter();
        TileObjectHandler handler = turn.actor.GetComponent<TileObjectHandler>();
        board.SelectTiles(tiles);
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("TIle Select State OnFire");
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
            {
                owner.ChangeState<TileHandleState>();
            }
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }
}