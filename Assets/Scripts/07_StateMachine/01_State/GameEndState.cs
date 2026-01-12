using Unity.VisualScripting;
using UnityEngine;

public class GameEndState : TurnState
{
    public override void Enter()
    {
        Debug.Log("Enter GameEndState");
        base.Enter();
        abilityMenuPanelController.MenuHide();
        abilityMenuPanelController.ResultShow(owner.turn.actor.name + "Win");
    }


}
