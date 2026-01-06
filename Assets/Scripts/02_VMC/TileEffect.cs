using UnityEngine;

// 타일 스크립트에 부착할 타일 효과 PlaceObject와 기본 기능을 공유하고자 함
// 이럴거면 그냥 인터페이스를 쓸걸 그랬나
public class TileEffect : PlaceObject
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

    public virtual void OnUnitEnter(Unit unit)
    {

    }
}
