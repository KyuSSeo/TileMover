using UnityEngine;

public class Tile : MonoBehaviour
{
    //  위치와 높이 정보
    public Point pos;
    public int height = 1;

    public GameObject content;
    //  높이 정보
    public const float stepHeight = 1f;
    
    public Vector3 center { get { return new Vector3(pos.x, height * stepHeight, pos.y); } }
    public Tile prevTile;
    public int distance;
    public TileType tileType = TileType.None;
    public int movementCost
    {
        get
        {
            switch (tileType)
            {
                case TileType.Normal: return 1;
                case TileType.Snow: return 2;
                case TileType.Bush: return 3;
                case TileType.Swamp: return 5;
                default: return 1;
            }
        }
    }
    public void UpdateColor()
    {
        Renderer tileColor = GetComponent<Renderer>();
        switch (tileType)
        {
            case TileType.Normal: tileColor.material.color = Color.gray; break;
            case TileType.Snow:   tileColor.material.color = Color.white; break;
            case TileType.Bush:   tileColor.material.color = Color.green; break;
            case TileType.Swamp:  tileColor.material.color = Color.black; break;
        }
    }
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
    public void Load(Point p, int tile)
    {
        pos = p;
        height = 1;
        tileType = (TileType)tile;
        Match();
        UpdateColor();
    }

    public void Load(Vector3 v)
    {
        Load(new Point((int)v.x, (int)v.z), (int)v.y);
    }

    // 시각적 업데이트를 위한 함수
    private void Match()
    {
        float currentHeight = height * stepHeight;
        transform.localPosition = new Vector3(pos.x, currentHeight / 2f, pos.y);
        transform.localScale = new Vector3(1, currentHeight, 1);
    }   
}
