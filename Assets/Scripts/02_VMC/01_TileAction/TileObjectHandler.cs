using UnityEngine;

public class TileObjectHandler : MonoBehaviour
{
    [SerializeField] public GameObject obstaclePrefab;

    public void BuildObstacle(Tile targetTile)
    {
        if (targetTile.content != null)
        {
            Debug.LogWarning("타일이 이미 점유중입니다.");
        }

        Point point = targetTile.pos;
        Debug.Log($"BuildObstacle 실행됨 {point.x}, {point.y}");
        /*
         * 타일 생성 로직
         * 장애물 Place,
         * 장애물 DirMatch 실행
         */
    }

    public void RemoveObstacle(Tile targetTile)
    {
        if (targetTile.content == null)
        {
            Debug.LogWarning("타일이 이미 비어있습니다.");
        }
        Point point = targetTile.pos;
        Debug.Log($"RemoveObstacle 실행됨 {point.x}, {point.y}");
        /*
         * 선택한 타일 위 오브젝트 Destroy 실행
         */
    }
}
