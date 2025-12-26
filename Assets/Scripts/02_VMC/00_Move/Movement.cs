using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class Movement : MonoBehaviour
{

    public int range;
    protected Unit unit;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }
    public virtual List<Tile> GetTilesInRange(Board board, Tile to)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch, to);

        if (retValue == null) Debug.Log("야! Search 결과가 NULL이다!");

        else if (retValue.Count == 0) Debug.Log("야! Search 결과가 0개다!");

        else Debug.Log("경로 찾음: " + retValue.Count + "칸");

        if (retValue != null)
        {
            Filter(retValue);
        }
        else
        {
            // 길 못 찾았으면 빈 리스트라도 만들어서 리턴해야 
            // 뒤쪽 로직에서 에러가 안 납니다.
            retValue = new List<Tile>();
        }

        return retValue;
    }

    public abstract IEnumerator Traverse(Tile tile);



    protected virtual bool ExpandSearch1(Tile from, Tile tile)
    {
        if (tile.content != null)
            return false;

        return (from.distance + tile.movementCost) <= range;
    }
    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return to.content == null;
    }

    protected virtual void Filter(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i >= 0; --i)
            if (tiles[i].content != null)
                tiles.RemoveAt(i);
    }

    protected virtual IEnumerator Turn(Directions dir)
    {
        yield return transform.DOLocalRotate(dir.ToEuler(), 0.25f)
            .SetEase(Ease.InOutQuad)
            .WaitForCompletion();
        unit.dir = dir;
    }
}