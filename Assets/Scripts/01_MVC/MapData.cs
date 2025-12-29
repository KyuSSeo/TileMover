using System.Collections.Generic;
using UnityEngine;

public class MapData : ScriptableObject
{
    // 타일 정보 (좌표, 타일 타입)
    public List<Vector3> tiles;
    // 타일 위 오브젝트 정보 ex) 지형지물
    public List<Vector3> units;
    // 유닛 스폰지점 
    public List<Vector3> spawnPoints;
}
