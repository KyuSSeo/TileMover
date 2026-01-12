using System;
using Unity.Behavior;
using Modifier = Unity.Behavior.Modifier;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Test Mod", story: "타일배치", category: "Action", id: "4bb8a81792a79fed7a71dcc3a76e2f90")]
public partial class TestModModifier : Modifier
{
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

