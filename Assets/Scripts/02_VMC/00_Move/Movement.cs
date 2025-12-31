using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class Movement : MonoBehaviour
{

    public int range;
    protected PlaceObjcet unit;

    protected virtual void Awake()
    {
        unit = GetComponent<PlaceObjcet>();
    }

    public virtual List<Tile> GetTilesInRange(Board board)
    {
        List<Tile> retValue = board.Search(unit.tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }


    public abstract IEnumerator Traverse(Tile tile);

    protected virtual bool ExpandSearch(Tile from, Tile tile)
    {
        if (tile.content != null)
            return false;

        return (from.distance + tile.movementCost) <= range;
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