using UnityEngine;

public class TileObjectHandler : MonoBehaviour
{
    [SerializeField] public GameObject obstaclePrefab;
    [SerializeField] private int wallDurability;

    public void BuildObstacle(Tile targetTile)
    {
        Point point = targetTile.pos;
        Debug.Log($"BuildObstacle 실행됨 {point.x}, {point.y}");
        
        if (targetTile.content != null)
        {
            Debug.LogWarning("타일이 이미 점유중입니다.");
            return;
        }

        GameObject instance = Instantiate(obstaclePrefab);
        Obstacle obj = instance.GetComponent<Obstacle>();
        obj.Place(targetTile);
        obj.SetDurability(wallDurability);
        obj.DirMatch();
    }

    public void RemoveObstacle(Tile targetTile)
    {
        if (targetTile.content == null)
        {
            Debug.LogWarning("타일이 이미 비어있습니다.");
        }
        Point point = targetTile.pos;
        Debug.Log($"RemoveObstacle 실행됨 {point.x}, {point.y}");
        Obstacle obj = targetTile.content.GetComponent<Obstacle>();
        if (obj != null) 
        {
            obj.DurabilityDown();
        }
        else 
        {
            Debug.LogWarning("잘못된 타겟 설정");
        }
    }
}
