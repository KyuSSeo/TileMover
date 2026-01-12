using UnityEngine;

//타일 효과를 상속받는 목적지 효과
public class GoalPoint : TileEffect
{
    public override void OnUnitEnter(Unit unit)
    {
        if (unit != null)
        {
            GameEnd();
        }
    }

    public bool GameEnd()
    {
        Debug.Log($"게임 종료조건 만족");
        return true;
    }
}
