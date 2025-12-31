using System.Collections;
using UnityEngine;


public class InitState : TurnState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(mapData);
        Point p = new Point((int)mapData.tiles[0].x, (int)mapData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits();
        yield return null;
        owner.ChangeState<SelectUnitState>();
    }

    void SpawnTestUnits()
    {
        for (int i = 0; i < mapData.spawnPoints.Count; i++)
        {
            // 좌표 설정
            int x = (int)mapData.spawnPoints[i].x;
            int y = (int)mapData.spawnPoints[i].z;
            Point pos = new Point(x, y);
            Tile targetTile = board.GetTile(pos);

            // 유닛 배치
            if (targetTile == null || targetTile.content != null)
            {
                Debug.LogWarning($"중복 배치 또는 잘못된 타일 ({x}, {y})");
                continue;
            }
            // 유닛 오브젝트 생성
            GameObject instance = Instantiate(owner.charactor) as GameObject;

            PlaceObjcet unit = instance.GetComponent<PlaceObjcet>();
            unit.Place(board.GetTile(pos));
            unit.DirMatch();

            // 이동거리 설정
            Movement moveRange = instance.AddComponent<WalkMovement>();
            moveRange.range = (int)mapData.spawnPoints[i].y;

            // 턴 목록에 추가
            units.Add(unit);
        }
    }
}