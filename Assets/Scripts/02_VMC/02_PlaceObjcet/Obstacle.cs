using Mono.Cecil;
using UnityEngine;

public class Obstacle : PlaceObject
{
    private int durability;

    public void SetDurability(int number) 
    {
        durability = number;
    }
    public void DurabilityDown()
    {
        if (durability-- <= 0)
        {
            DestroyObj();
        }
        else
        {
            durability--;
            Debug.Log($"배치 오브젝트 내구도 : {durability}");
        }
    }
}
