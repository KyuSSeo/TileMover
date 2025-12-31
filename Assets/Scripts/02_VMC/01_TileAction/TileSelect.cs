using UnityEngine;

public class TileSelect : MonoBehaviour
{
    public int range;
    protected Unit unit;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
}
