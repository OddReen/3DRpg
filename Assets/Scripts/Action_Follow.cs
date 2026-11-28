using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Follow", story: "Follow", category: "Action", id: "42ad5e1cecbc28ba8f48a0f48f6cdfcc")]
public partial class Action_Follow : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;

    public Transform target;
    public float stoppingDistance;
    public float maxDistance;

    protected override Status OnStart()
    {
        Agent.Value.stoppingDistance = stoppingDistance;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Vector3.Distance(Self.Value.transform.position, target.position) > maxDistance)
        {
            Agent.Value.SetDestination(target.position);
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {

    }
}

