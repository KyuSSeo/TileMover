using UnityEngine;


public enum Directions
{
    North = 0,
    East  = 1,
    South = 2,
    West  = 3,
}

public enum TileType
{
    None = 0,
    Normal = 1,
    Snow = 2,
    Bush = 3,
    Swamp = 4,
}

public enum TilePathFinding
{
    None = 0,
    SelectedPath = 1,
    ClosePath = 2,
}


[System.Flags]
public enum States
{
    None = 0,
    Selected = 1 << 0,
    Locked = 1 << 1
}

