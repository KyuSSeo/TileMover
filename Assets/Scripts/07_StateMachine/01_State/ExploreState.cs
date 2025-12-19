using UnityEngine;
using System.Collections;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

public class ExploreState : TurnState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}