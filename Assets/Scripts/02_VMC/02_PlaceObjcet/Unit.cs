using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

// РЏДж
public class Unit : PlaceObject
{
    public override void Place(Tile target)
    {
        base.Place(target);

        if (target != null && target.tileEffect != null)
        {
            target.tileEffect.OnUnitEnter(this);
        }
    }
}
