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
                Debug.LogWarning($"중복 배치 또는 잘못된 타일 ({x}, {z})");
                continue;
            }
            // 유닛 오브젝트 생성
            GameObject instance = Instantiate(owner.charactor) as GameObject;

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(pos));
            unit.DirMatch();

            // 이동거리 설정
            Movement moveRange = instance.AddComponent<WalkMovement>();
            moveRange.range = (int)mapData.spawnPoints[i].y;

            // 턴 목록에 추가
            units.Add(unit);
        }

        /* 기존 코드
        System.Type[] components = new System.Type[] { typeof(WalkMovement), typeof(WalkMovement) };
        for (int i = 0; i < 2; ++i)
        {
            GameObject instance = Instantiate(owner.charactor) as GameObject;

            Point p = new Point((int)mapData.tiles[i].x, (int)mapData.tiles[i].z);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.DirMatch();

            Movement m = instance.AddComponent(components[i]) as Movement;
            // 이동거리 
            m.range = 500;
            units.Add(unit);
        }
        */
    }
}