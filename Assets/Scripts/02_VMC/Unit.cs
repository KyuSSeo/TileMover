using UnityEngine;

public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;

    public void Place(Tile target)
    {
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    // 타일 점유중인 유닛 제거시 타일정보 갱신
    public void UnitRemoved()
    {
        if (tile != null && tile.content == gameObject)
        {
            tile.content = null;
        }
    }

    public void DirMatch()
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}