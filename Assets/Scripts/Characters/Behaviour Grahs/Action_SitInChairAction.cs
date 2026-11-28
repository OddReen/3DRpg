using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEditor.VersionControl;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Action_SitInChair", story: "Sit In Chair", category: "Action", id: "3a5cf4bc7bf0571af50b847c084a8116")]
public partial class Action_SitInChairAction : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Agent;
    [SerializeReference] public BlackboardVariable<Structure_Chair> Target;

    bool wasSat = false;

    protected override Status OnStart()
    {
        if (Target.Value.ocuppied)
            return Status.Failure;

        wasSat = true;

        Target.Value.ocuppied = true;
        Agent.Value.agent.enabled = false;
        Agent.Value.animator.CrossFade("SatInChair", .1f);

        Vector3 rotate = new Vector3(0.0f, Target.Value.transform.rotation.eulerAngles.y, 0.0f);
        Agent.Value.transform.SetPositionAndRotation(Target.Value.transform.position, Quaternion.Euler(rotate));
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (!wasSat)
            return;

        Target.Value.ocuppied = false;
        Agent.Value.agent.enabled = true;
    }
}

