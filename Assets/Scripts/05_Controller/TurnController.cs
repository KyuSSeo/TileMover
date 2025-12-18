using UnityEngine;

public class TurnController : StateMachine
{
	public CamRig camRig;
    public Board board;
    public MapData mapData;
    public Transform tileSelectionIndicator;
    public Point pos;
    public GameObject charactor;
    public Unit currentUnit;

    public Tile currentTile { get { return board.GetTile(pos); } }
    void Start()
    {
        ChangeState<InitState>();
    }
}