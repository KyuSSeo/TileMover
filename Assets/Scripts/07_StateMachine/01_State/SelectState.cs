using UnityEngine;

public class SelectUnitState : TurnState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        GameObject content = owner.currentTile.content;
        if (content != null)
        {
            owner.currentUnit = content.GetComponent<Unit>();
            owner.ChangeState<MoveToTargetState>();
        }
    }
}