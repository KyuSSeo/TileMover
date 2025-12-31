using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;

    private void OnDestroy()
    {
        UnitRemoved();
    }

    // 타일 점유
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

    // 타일 위 위치, 방향 조정
    public void DirMatch()
    {
        transform.localPosition = tile.center + new Vector3(0, 0.5f, 0);
        transform.localEulerAngles = dir.ToEuler();
    }
}