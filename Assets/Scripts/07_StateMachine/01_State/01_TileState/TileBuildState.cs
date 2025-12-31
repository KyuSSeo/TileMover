using System.Collections.Generic;
using UnityEngine;

public class TileBuildState : TurnState
{
    List<Tile> tiles;
    TileObjectHandler handler;

    public override void Enter()
    {
        Debug.Log("Tile Build Enter");
        base.Enter();
        handler = turn.actor.GetComponent<TileObjectHandler>();
        board.SelectTiles(tiles);
        if (tiles != null && tiles.Count > 0)
            board.SelectTiles(tiles);
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Tile Build OnFire");
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
            {
                owner.ChangeState<CommandSelectionState>();
                handler.BuildObstacle(owner.currentTile);
                turn.hasUnitActed = true;
            }
        }
        else
        {
            owner.ChangeState<TileeHandleSelectState>();
        }
    }
}