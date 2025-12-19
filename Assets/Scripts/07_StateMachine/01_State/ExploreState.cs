using UnityEngine;
using System.Collections;

public class ExploreState : TurnState
{
    public override void Enter()
    {
        base.Enter();
        // 커서 활성화 등 초기화
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        // 카메라나 커서 이동 로직
        SelectTile(e.info + pos);
    }
}