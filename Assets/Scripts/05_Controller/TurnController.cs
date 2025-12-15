using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;
using static UnityEngine.UI.GridLayoutGroup;
public class TurnController : StateMachine
{
    public Board board;
    public MapData mapData;
    public Transform tileSelectionIndicator;
    public Point pos;

    void Start()
    {
        //ChangeState<>();
    }
}