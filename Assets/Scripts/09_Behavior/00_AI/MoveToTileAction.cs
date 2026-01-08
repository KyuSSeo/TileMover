using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTileAction ", story: "이동", category: "Action", id: "0b76fb057e9cbd8fb052f205cd7253d1")]
public partial class MoveToTileAction : Action
{
    [SerializeReference] public BlackboardVariable<Unit> Agent;
    [SerializeReference] public BlackboardVariable<Tile> Target;

    private WalkMovement movement;
    private bool isMoving = false;


    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
            return Status.Failure;

        movement = Agent.Value.GetComponent<WalkMovement>();

        if (movement == null) 
            return Status.Failure;

        Agent.Value.StartCoroutine(movement.Traverse(Target.Value));
        isMoving = true;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value.transform.position == Target.Value.center)
        {
            isMoving = false;
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        isMoving = false;
    }
}

