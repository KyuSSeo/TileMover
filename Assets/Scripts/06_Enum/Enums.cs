using UnityEngine;


public enum Directions
{
    North = 0,
    East  = 1,
    South = 2,
    West  = 3,
}
public enum States
{
    None = 0,
    Selected = 1 << 0,
    Locked = 1 << 1
}

