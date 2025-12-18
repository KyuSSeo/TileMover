using UnityEngine;

public class Tile : MonoBehaviour
{
    //  위치와 높이 정보
    public Point pos;
    public int height;

    public GameObject content;
    //  높이 정보 4단계
    public const float stepHeight = 1f;
    
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }
    
    public Tile prevTile;
    public int distance;
    
    //  타일 변형
    public void Grow()
    {
        height++;
        Match();
    }

    public void Shrink()

    {
        height--;
        Match();
    }

    //  타일 정보 가져오기
    public void Load(Point p, int h)
    {

        pos = p;
        height = h;
        Match();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }

    // 시각적 업데이트를 위한 함수
    private void Match()
    {
        transform.localPosition = new Vector3(pos.x, height * stepHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, height * stepHeight, 1);
    }
}
