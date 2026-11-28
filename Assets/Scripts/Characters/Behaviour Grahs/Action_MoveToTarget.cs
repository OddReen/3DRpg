using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "Move To Target", category: "Action", id: "62233eacacbcb92a003340170ea1be86")]
public partial class Action_MoveToTarget : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> stoppingDistance;

    protected override Status OnStart()
    {
        if (Target.Value == null)
            return Status.Failure;
        Agent.Value.PlayMovementStance();
        Agent.Value.agent.stoppingDistance = stoppingDistance;
        Agent.Value.agent.SetDestination(Target.Value.transform.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.RotateToMoveDirectionSmoothed(Agent.Value.agent.velocity.normalized);

        Agent.Value.agent.SetDestination(Target.Value.transform.position);

        if (Agent.Value.HasArrived())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

