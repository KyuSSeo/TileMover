using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

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



    /*
     *  경로 탐색 방법은?
     *  Tile스크립트는 Unit스크립트를 통해 자기 위에 뭐가 있는지 알고 있다.
     *  Board는 모든 타일을 딕셔너리로 알고 있다.
     *  
     *  목표, 골인 지점을 설정한다.
     *  Tile에 현재 Point기준  x1 y0, x0 y1, x-1 y0, x0 y-1은 으로 차이나는 Point는 다음 경로 탐색에 사용되는 노드로 작동함 
     *  Tile위의 Content Object를 확인하여 다음 타일이 갈 수 있는지 여부를 체크합니다.
     *  
     *
     */
}