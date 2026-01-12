using Unity.VisualScripting;
using UnityEngine;

public class GameEndState : TurnState
{
    public override void Enter()
    {
        Debug.Log("게임종료 상태 진입");
        abilityMenuPanelController.ResultShow(owner.turn.actor.name + "Win");
    }
}
