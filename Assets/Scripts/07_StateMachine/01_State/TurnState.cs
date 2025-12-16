using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;
using Unity.VisualScripting;

public abstract class TurnState : State
{
    protected TurnController owner;
    public Board board { get { return owner.board; } }
    public MapData mapData { get { return owner.mapData; } }
    public Transform tileSelectionIndicator { get { return owner.tileSelectionIndicator; } }
    public Point pos { get { return owner.pos; } set { owner.pos = value; } }

    protected virtual void Awake()
    {
        owner = GetComponent<TurnController>();
    }

    protected override void AddListeners()
    {
        InputController.moveEvent += OnMove;
        InputController.selEvent += OnSelect;
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.selEvent -= OnSelect;
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnSelect(object sender, InfoEventArgs<int> e)
    {

    }

    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;

        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }
}