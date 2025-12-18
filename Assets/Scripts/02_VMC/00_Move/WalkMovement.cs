using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        if (to.content != null)
            return false;

        return base.ExpandSearch(from, to);
    }
    public override IEnumerator Traverse(Tile tile)
    {
        unit.Place(tile);

        List<Tile> targets = new List<Tile>();
        while (tile != null)
        {
            targets.Insert(0, tile);
            tile = tile.prevTile;
        }

        for (int i = 1; i < targets.Count; ++i)
        {
            Tile from = targets[i - 1];
            Tile to = targets[i];

            Directions dir = from.GetDirection(to);
            if (unit.dir != dir)
                yield return StartCoroutine(Turn(dir));

            yield return StartCoroutine(Walk(to));
        }
        yield return null;
    }

    IEnumerator Walk(Tile target)
    {
        yield return transform.DOMove(target.center, 0.5f)
            .SetEase(Ease.Linear)
            .WaitForCompletion();
    }
}