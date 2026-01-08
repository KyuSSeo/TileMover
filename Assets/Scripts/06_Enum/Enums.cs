using UnityEngine;


// 유닛의 방향
public enum Directions
{
    North = 0,
    East  = 1,
    South = 2,
    West  = 3,
}

// 타일의 이동 소모값
public enum TileType
{
    None = 0,
    Normal = 1,
    Snow = 2,
    Bush = 3,
    Swamp = 4,
}

// 이동 알고리즘 테스트용 변수
public enum TilePathFinding
{
    None = 0,
    SelectedPath = 1,
    ClosePath = 2,
}

public enum PlayerType
{
    None,
    Human,
    Ai,
}

// 행동여부 판단용도
[System.Flags]
public enum States
{
    None = 0,
    Selected = 1 << 0,
    Locked = 1 << 1
}