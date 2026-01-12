using System.Collections;


//  이동 명령을 결정지었을 때, 이동하는 중 입력방지
public class MoveSequenceState : TurnState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
        owner.camRig.FollowTarget(owner.turn.actor.transform);
        isGameEnd = CheckGameEnd();
    }

    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        turn.hasUnitMoved = true;
        if (isGameEnd == true)
        {
            // 이동 완료시 게임종료로 전이
            owner.ChangeState<GameEndState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }

    private bool CheckGameEnd()
    {
        Tile currentTile = owner.currentTile;
        if (currentTile != null && currentTile.tileEffect != null)
        {
            if (currentTile.tileEffect is GoalPoint)
            {
                return true;
            }
        }
        return false;
    }
}