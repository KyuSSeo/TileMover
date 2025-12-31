using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileActionState : TurnState
{
    TileObjectHandler handler;
    List<Tile> tiles; // 선택된 타일 표시용 리스트

    public override void Enter()
    {
        base.Enter();
        handler = turn.actor.GetComponent<TileObjectHandler>();

        string mode = (owner.subCategory == 0) ? "Build" : "Remove";
        Debug.Log($"Tile Action State Enter: {mode}");

        SelectTile(pos);
        tiles = new List<Tile>();
        RefreshRange();
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles.Clear();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshRange();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
            {
                if (owner.subCategory == 0)
                {
                    handler.BuildObstacle(owner.currentTile);
                }
                else if (owner.subCategory == 1)
                {
                    handler.RemoveObstacle(owner.currentTile);
                }

                turn.hasUnitActed = true;
                owner.ChangeState<CommandSelectionState>();
            }
        }
        else
        {
            owner.ChangeState<TileeHandleSelectState>();
        }
    }
    void RefreshRange()
    {
        board.DeSelectTiles(tiles);
        tiles.Clear();
        tiles.Add(owner.currentTile);
        board.SelectTiles(tiles);
    }
}