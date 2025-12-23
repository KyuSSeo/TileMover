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
            m.range = 10;
            units.Add(unit);
        }
    }
}