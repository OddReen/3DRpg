using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToRandomPosition", story: "Move To Random Position", category: "Action", id: "28022a6abb38ea724f6e2d2b15e848f7")]
public partial class Action_MoveToRandomPosition : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Agent;
    [SerializeReference] public BlackboardVariable<float> range;
    [SerializeReference] public BlackboardVariable<float> stoppingDistance;

    protected override Status OnStart()
    {
        Agent.Value.agent.stoppingDistance = stoppingDistance;
        Vector3 randPoint = Agent.Value.transform.position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            Agent.Value.agent.SetDestination(hit.position);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.RotateToMoveDirectionSmoothed(Agent.Value.agent.velocity.normalized);
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

