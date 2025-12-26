using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    [SerializeField] GameObject tileViewPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;
    [SerializeField] GameObject unitPrefab; 

    private Transform _marker;
    #region transform marker
    public Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
        }
    }
    #endregion

    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    public Dictionary<Point, Unit> units = new Dictionary<Point, Unit>();

    [SerializeField] public int width = 10;
    [SerializeField] public int depth = 10;
    [SerializeField] public int height = 1;
    [SerializeField] public Point pos;
    [SerializeField] public MapData mapData;

    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }


    public void Grow()
    {
        GrowSingle(pos);
    }

    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    public void AddUnit()
    {
        CreateUnit(pos);
    }

    public void RemoveUnit()
    {
        DeleteUnit(pos);
    }
    public void RandomizeTileType()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.tileType = (TileType)Random.Range(0, 4);
            tile.UpdateColor();
        }
    }

    public void UpdateMarker()
    {
        Tile tile = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = tile != null ? tile.center : new Vector3(pos.x, 0, pos.y);
    }

    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        tiles.Clear();
        units.Clear();
    }

    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        MapData board = ScriptableObject.CreateInstance<MapData>();
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile tile in tiles.Values)
            board.tiles.Add(new Vector3(tile.pos.x, (float)tile.tileType, tile.pos.y));

        board.units = new List<Vector3>(units.Count);
        foreach (Unit unit in units.Values)
        {
            board.units.Add(new Vector3(unit.tile.pos.x, 0, unit.tile.pos.y));
        }

        string fileName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    //  불러오기
    public void Load()
    {
        Clear();
        if (mapData == null)
            return;

        foreach (Vector3 v in mapData.tiles)
        {
            Tile t = Create();
            Point p = new Point((int)v.x, (int)v.z);

            t.Load(p, 0);
            t.tileType = (TileType)(int)v.y;
            t.UpdateColor();

            tiles.Add(t.pos, t);
        }

        if (mapData.units != null)
        {
            foreach (Vector3 v in mapData.units)
            {
                Point p = new Point((int)v.x, (int)v.z);
                CreateUnit(p); // 해당 위치에 유닛 생성
            }
        }
    }


    public Rect RandomRect()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, depth);
        int w = Random.Range(1, width - x + 1);
        int h = Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }

    public void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                if (tiles.ContainsKey(p))
                    continue;
                GrowSingle(p);
            }
        }
    }

    public void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    public Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    public Unit CreateUnit(Point p)
    {
        if (!tiles.ContainsKey(p))
            return null;

        if (units.ContainsKey(p))
            return units[p];

        GameObject instance = Instantiate(unitPrefab) as GameObject;
        instance.transform.parent = transform;

        Unit unit = instance.GetComponent<Unit>();
        Tile t = tiles[p];

        unit.Place(t);
        units.Add(p, unit);

        t.content = instance;

        return unit;
    }

    public void DeleteUnit(Point p)
    {
        if (!units.ContainsKey(p))
            return;

        Unit unit = units[p];

        if (unit.tile != null) unit.tile.content = null;

        units.Remove(p);
        DestroyImmediate(unit.gameObject);
    }

    public Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    public void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

    public void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }
    // 맵 전체 생성 및 랜덤 타입 적용
    public void GenerateMap()
    {
        Clear();

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < depth; ++y)
            {
                Point p = new Point(x, y);

                // 타일 생성
                GrowSingle(p);

                // 랜덤 타일 타입 적용
                if (tiles.ContainsKey(p))
                {
                    Tile t = tiles[p];
                    t.tileType = (TileType)Random.Range(0, 5);
                }
            }
        }
    }

    //  맵 저장
    public void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }
}
