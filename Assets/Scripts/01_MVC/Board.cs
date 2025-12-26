using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Profiling;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject wallPrefab;

    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    private Color selectedTileColor = new Color(0, 1, 1, 1);
    private Color defaultTileColor = new Color(1, 1, 1, 1);


    // 시간체크용 변수
    private Stopwatch stopwatch = new Stopwatch();

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
                GameObject instance = Instantiate(wallPrefab) as GameObject;
                Unit u = instance.GetComponent<Unit>();

                Point p = new Point((int)data.units[i].x, (int)data.units[i].z);

                if (tiles.ContainsKey(p))
                {
                    Tile t = tiles[p];
                    u.Place(t);
                }
            }
        }
    }
    private void ClearSearch()
    {
        stopwatch.Restart();
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

    #region BFS

    // 탐색 단계에서 Distance 를 Range 만큼 제한해준다.

    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();

        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);
        // 시작점 결정: start.distance = 0 설정 후 checkNow에 추가.
        while (checkNow.Count > 0)
        {
            // 타일 확인 및 이웃 추가: checkNow에서 하나 꺼내(Dequeue) 4방향을 검사하고, 조건에 맞으면 checkNext에 넣습니다.

            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                // 방문한 타일 건너뛰기
                if (next == null || next.distance <= t.distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    // "추가된 모든 타일의 거리는... 1만큼 더 크게 설정" → next.distance = t.distance + 1
                    // (기존) next.distance = t.distance + 1;

                    // 타일 타입에 따라 이동경로 가중치 
                    next.distance = t.distance + next.movementCost;

                    // "현재 타일은 이전 타일의 참조로 설정" → next.prevTile = t
                    next.prevTile = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }
            }
            // 대기열 교환(Swap) : checkNow가 비었을 때(Count == 0) checkNext와 역할을 바꿉니다.

            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
            // 루프 계속: checkNow에 아직 타일이 남아있다면 Swap 하지 않고 while문을 계속 돕니다.
        }

        // 타이머 멈춤
        stopwatch.Stop();
        // 결과 확인 (밀리초 단위)
        long pathFindTimeMs = stopwatch.ElapsedMilliseconds;
        // 또는 정밀한 시간 간격 객체(TimeSpan)로 받기
        TimeSpan timeSpan = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"소모시간(Ms) : {pathFindTimeMs}");
        UnityEngine.Debug.Log($"소모시간(Ticks) : {timeSpan}");
        return retValue;
    }

    private void SwapReference(ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }


    #endregion

    #region DFS

    public List<Tile> Search1(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();

        Stack<Tile> stack = new Stack<Tile>();

        start.distance = 0;
        stack.Push(start);

        while (stack.Count > 0)
        {
            // 가장 최근에 넣은 타일을 꺼냄
            Tile t = stack.Pop();
            retValue.Add(t);

            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                if (next == null || next.distance <= t.distance + 1)
                    continue;

                // distance 확인
                if (next != null && next.distance == int.MaxValue)
                {
                    if (addTile(t, next))
                    {

                        //next.distance = t.distance + 1;
                        next.distance = t.distance + t.movementCost;
                        next.prevTile = t;

                        // 발견 즉시 스택에 넣음 -> 다음 반복에서 바로 꺼내짐
                        stack.Push(next);
                    }
                }
            }
        }
        // 타이머 멈춤
        stopwatch.Stop();
        // 결과 확인 (밀리초 단위)
        long pathFindTimeMs = stopwatch.ElapsedMilliseconds;
        // 또는 정밀한 시간 간격 객체(TimeSpan)로 받기
        TimeSpan timeSpan = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"소모시간(Ms) : {pathFindTimeMs}");
        UnityEngine.Debug.Log($"소모시간(Ticks) : {timeSpan}");
        return retValue;
    }
    #endregion

    #region Dijkstra

    /* 다익스트라 알고리즘 작동원리 알아보기
     * 베이스 : 우리가 찾는 최단 경로는 여러 가짓수의 최단 경로로 이루어져 있다.
     * 가는 거리가 음수인 노드는 존재하지 않는 경우
     * 경로탐색을 통해서 최단 거리를 갱신하는 방식이다.
     * 
     * 1. 출발 노드 설정 : 내 프로젝트의 경우 시작점, Unit스크립트가 점유중인 타일
     * 2. 출발 노드 기준 최소 비용 노드 설정 : 노드 비용은 모두 1로 같다
     * 3. 방문하지 않은 노드 중 가장 저렴한 노드 방문 : 노드 비용이 모두 1로 같은데
     * 4. 해당 노드를 거쳐 특정한 노드로 가는 경우에 비용을 갱신
     * 5. 위 과정에서 3 ~ 4를 반복 
     *
     */
    public List<Tile> Search2(Tile start, Func<Tile, Tile, bool> addTile)
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
        // 타이머 멈춤
        stopwatch.Stop();
        // 결과 확인 (밀리초 단위)
        long pathFindTimeMs = stopwatch.ElapsedMilliseconds;
        // 또는 정밀한 시간 간격 객체(TimeSpan)로 받기
        TimeSpan timeSpan = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"소모시간(Ms) : {pathFindTimeMs}");
        UnityEngine.Debug.Log($"소모시간(Ticks) : {timeSpan}");
        return retValue;
    }
    #endregion

    #region A*
    /*
     *  A* 알고리즘 알아보기
     *   다익스트라 알고리즘에 휴리스틱(추정치)을 더해 목표 지점까지의 경로를 더 효율적으로 탐색
     *
     *   g = 출발지점부터 현재 위치한 노드까지 최소 비용
     *   h = 현재 위치 노드부터 목표 노드까지 예상 비용
     *   f = 현재 위치 비용 + 목표 노드까지 예상비용
     *
     *  두개의 리스트를 사용합니다.
     *  탐색한 노드와 탐색하지 않은 노드
     *  
     *  현재 노드에서 모든 경로를 탐색합니다. 
     *  탐색된 다음 노드 중 가장 비용이 적은 노드를 다음 노드로 설정합니다.
     *  기존 노드는 탐색 완료 노드 리스트에 추가됩니다.
     *  
     */
    public List<Tile> Search3(Tile start, Tile end, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        // 탐색 대기 노드
        List<Tile> openList = new List<Tile>();
        // 탐색 완료 노드
        List<Tile> closedList = new List<Tile>();
        start.distance = 0;
        openList.Add(start);

        while (openList.Count > 0)
        {
            openList.Sort((x, y) =>
            {
                // F = G(distance) + H(Heuristic)
                int f_x = x.distance + GetHeuristic(x, end);
                int f_y = y.distance + GetHeuristic(y, end);
                return f_x.CompareTo(f_y);
            });

            Tile t = openList[0];
            openList.RemoveAt(0);
            // 목적지
            if (t == end)
            {
                retValue.Add(t);
                return retValue;
            }
            // 방문 완료 처리
            closedList.Add(t);

            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.pos + dirs[i]);

                // 갈 수 없거나, 이미 완전히 파악된 타일
                if (next == null || closedList.Contains(next) || !addTile(t, next))
                    continue;

                int newG = t.distance + next.movementCost;

                // 더 좋은 경로를 찾았거나, 처음 방문하는 경우

                if (newG < next.distance || !openList.Contains(next))
                {
                    next.distance = newG;          // G 갱신
                    next.prevTile = t;             // 족보 갱신

                    if (!openList.Contains(next))
                        openList.Add(next);
                }
            }
        }
        // 타이머 멈춤
        stopwatch.Stop();
        // 결과 확인 (밀리초 단위)
        long pathFindTimeMs = stopwatch.ElapsedMilliseconds;
        // 또는 정밀한 시간 간격 객체(TimeSpan)로 받기
        TimeSpan timeSpan = stopwatch.Elapsed;
        UnityEngine.Debug.Log($"소모시간(Ms) : {pathFindTimeMs}");
        UnityEngine.Debug.Log($"소모시간(Ticks) : {timeSpan}");
        return retValue;
    }

    private int GetHeuristic(Tile a, Tile b)
    {
        return Mathf.Abs(a.pos.x - b.pos.x) + Mathf.Abs(a.pos.y - b.pos.y);
    }
    #endregion
}