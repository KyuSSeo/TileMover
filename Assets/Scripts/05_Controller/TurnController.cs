using UnityEngine;

public class TurnController : StateMachine
{
	public CamRig camRig;
    public Board board;
    public MapData mapData;
    public Transform tileSelectionIndicator;
    public Point pos;

    void Start()
    {
        ChangeState<InitState>();
    }
}