using System;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject wallPrefab;

    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    private Color selectedTileColor = new Color(0, 1, 1, 1);
    private Color defaultTileColor = new Color(1, 1, 1, 1);

    private readonly Point[] dirs = new Point[4]
    {
        new Point(0, 1),
        new Point(0, -1),
        new Point(1, 0),
        new Point(-1, 0)
    };

    public void Load(MapData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject instance = Instantiate(tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(data.tiles[i]);
            tiles.Add(t.pos, t);
        }
        if (data.units != null)
        {
            for (int i = 0; i < data.units.Count; ++i)
            {
                // 좌표 설정
                int x = (int)data.units[i].x;
                int z = (int)data.units[i].z;
                Point pos = new Point(x, z);

                Tile targetTile = GetTile(pos);

                // 예외 처리
                if (targetTile == null || targetTile.content != null)
                {
                    UnityEngine.Debug.LogWarning($"지형지물 중복/오류 ({x}, {z})");
                    continue;
                }

                // 생성
                GameObject instance = Instantiate(wallPrefab) as GameObject;
                Unit obj = instance.GetComponent<Unit>();

                // 타일 점유 설정
                obj.Place(targetTile);
                obj.DirMatch();
            }
        }
    }

    private void ClearSearch()
    {
        foreach (Tile t in tiles.Values)
        {
            t.prevTile = null;
            t.distance = int.MaxValue;
        }
    }
    public Tile GetTile(Point p)
    {
        return tiles.ContainsKey(p) ? tiles[p] : null;
    }

    public void SelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    public void DeSelectTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
    }

    #region Dijkstra
    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();

        List<Tile> openList = new List<Tile>();
        start.distance = 0;
        openList.Add(start);

        while (openList.Count > 0)
        {
            // 가장 가까운 타일 정렬
            openList.Sort((x, y) => x.distance.CompareTo(y.distance));

            Tile t = openList[0];
            openList.RemoveAt(0);

            // 결과 리스트에 없으면 추가
            if (!retValue.Contains(t))
                retValue.Add(t);

            // 연결된 노드 방문
            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                if (next == null || !addTile(t, next))
                    continue;

                // 연결된 노드의 비용은 기존 비용 +1로 갱신
                //(기존) int newCost = t.distance + 1;

                // 비용은 타일 타입에 따라 갱신
                int newCost = t.distance + next.movementCost;

                if (addTile(t, next))
                {

                    // 경로 갱신
                    if (newCost < next.distance)
                    {
                        next.distance = newCost;
                        next.prevTile = t;

                        if (!openList.Contains(next))
                            openList.Add(next);
                    }
                }
            }
        }
        return retValue;
    }
    #endregion
}