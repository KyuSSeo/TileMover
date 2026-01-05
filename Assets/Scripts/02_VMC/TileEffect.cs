using UnityEngine;

// 타일 스크립트에 부착할 타일 효과
public class TileEffect : MonoBehaviour
{
    public Tile tile { get; protected set; }

    public void EffPlace(Tile target)
    {
        if (tile != null && tile.tileEffect == this)
            tile.tileEffect = null;
        
        tile = target;

        if (target != null)
            target.tileEffect = this;
    }

    public virtual void OnUnitEnter(Unit unit)
    {

    }
}
