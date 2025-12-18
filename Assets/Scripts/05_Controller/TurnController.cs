using System.Collections.Generic;
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
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    public Tile currentTile { get { return board.GetTile(pos); } }
    void Start()
    {
        ChangeState<InitState>();
    }
}