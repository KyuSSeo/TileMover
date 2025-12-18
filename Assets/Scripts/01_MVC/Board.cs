using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
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
    }

    #region BFS

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.distance = t.distance + 1;
                    next.prevTile = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }
        }

        return retValue;
    }

    private void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
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
    #endregion

    /*
     * 
     *  경로 탐색 방법은?
     *  Tile스크립트는 Unit스크립트를 통해 자기 위에 뭐가 있는지 알고 있다.
     *  Board는 모든 타일을 딕셔너리로 알고 있다.
     *  
     *  목표, 골인 지점을 설정한다.
     *  Tile에 현재 Point기준  x1 y0, x0 y1, x-1 y0, x0 y-1은 으로 차이나는 Point는 다음 경로 탐색에 사용되는 노드로 작동함 
     *  Tile위의 Content Object를 확인하여 다음 타일이 갈 수 있는지 여부를 체크합니다.
     *  
     *  필요한 정보는 뭐가 있을까? 위치한 타일부터 목표 타일까지의 거리
     *  위치한 타일 정보, 목표로 한 타일 정보
     *  타일 위에 오브젝트의 존재 유무?
     *  
     *  BFS (너비 우선 탐색): 시작 노드에서 가까운 노드부터 차례로 탐색하여 최단 경로(간선 수가 가장 적은 경로)를 찾습니다. 
     *  모든 가능한 경로를 탐색할 때 유용합니다.
     *  
     *  DFS (깊이 우선 탐색):특정 경로를 끝까지 탐색한 후, 막히면 이전 노드로 돌아와 다른 경로를 탐색합니다. 
     *  미로 찾기 등 경로의 존재 여부를 확인할 때 사용됩니다.
     *  
     *  다익스트라 (Dijkstra) 알고리즘 : 음수 가중치가 없는 그래프에서 단일 시작점에서 다른 모든 정점까지의 최단 경로를 찾습니다. 
     *  내비게이션의 빠른 길 찾기에 가장 많이 쓰입니다.
     *  
     *  A* 알고리즘 : 다익스트라 알고리즘에 휴리스틱(추정치)을 더해 목표 지점까지의 경로를 더 효율적으로 탐색합니다. 
     *  게임 인공지능 등에 활용됩니다.
     *  
     *  두개의 리스트를 사용합니다.
     *  탐색한 노드와 탐색하지 않은 노드
     *  
     *  현재 노드에서 모든 경로를 탐색합니다. 
     *  탐색된 다음 노드 중 가장 비용이 적은 노드를 다음 노드로 설정합니다.
     *  기존 노드는 탐색 완료 노드 리스트에 추가됩니다.
     *  
     *  이 과정을 반복합니다.
     *  
     *  목적지에 도달하면 반복을 종료합니다.
     *  
     *  대각선 이동을 지원하지 않기 때문에 최단 경로 = 이동한 타일의 개수입니다.
     *  타일의 좌표가 x,y로 되어있기 때문에
     *  시작점 S가 있을 때, 도착점 A와의 거리는 |Sx - Ax| + |Sy - Ay| = H
     *  
     *  (Current Cost): 시작점에서 현재 타일까지 오는데 걸린 실제 비용 (이동한 타일 수). G
     *  (Heuristic): 현재 타일에서 목표까지 남은 예상 거리 (맨해튼 거리). H
     *  (Total Cost): $G + H$. 이 값이 가장 낮은 노드를 다음 탐색 대상으로 선정합니다. F
     *  
     */
}