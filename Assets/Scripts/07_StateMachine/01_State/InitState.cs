using System.Collections;


public class InitState : TurnState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(mapData);
        Point p = new Point((int)mapData.tiles[0].x, (int)mapData.tiles[0].z);
        SelectTile(p);
        yield return null;
        owner.ChangeState<InitState>();
    }
}