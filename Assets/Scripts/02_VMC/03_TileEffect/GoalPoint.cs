using UnityEngine;

//타일 효과를 상속받는 목적지 효과
public class GoalPoint : TileEffect
{
    public override void OnUnitEnter(Unit unit)
    {
        base.OnUnitEnter(unit);

        if (unit != null)
        {
            Debug.Log($"게임 종료 {unit.name}가 목적지에 도착했습니다.");

            GameEnd();
        }
    }

    public void GameEnd()
    {

    }
}
