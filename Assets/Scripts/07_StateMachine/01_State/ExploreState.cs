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

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0) // 선택 키
        {
            // 현재 타일에 유닛이 있으면 CommandState로 전환
            var unit = mapData.GetUnit(pos);
            if (unit != null && !unit.turn.IsLocked) // 행동 안 한 유닛이면
            {
                owner.currentUnit = unit;
                owner.ChangeState<CommandState>();
            }
        }
    }
}
}