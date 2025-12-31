using System.Collections.Generic;
using UnityEngine;

public class TileHandleState : TurnState
{
    List<Tile> tiles;

    public override void Enter() 
    {
        Debug.Log("Tile Handle State Enter");
        base.Enter();
    }
}
