public abstract class TileEffect : PlaceObject
{
    public override void Place(Tile target)
    {
        if (tile != null && tile.tileEffect == this)
            tile.tileEffect = null;

        tile = target;

        if (target != null)
            target.tileEffect = this;
    }

    public override void ObjectRemoved()
    {
        if (tile != null && tile.tileEffect == gameObject)
        {
            tile.tileEffect = null;
        }
    }
    public abstract void OnUnitEnter(Unit unit);
}
