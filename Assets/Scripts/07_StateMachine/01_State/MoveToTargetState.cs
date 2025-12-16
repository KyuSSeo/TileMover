using UnityEngine;


public class MoveToTargetState : TurnState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }
}