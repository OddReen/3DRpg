using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Nearby Chair Has Target", story: "Set Nearby Chair As Target", category: "Action", id: "a6f4b3168fb5e3f2a07276f34f41b417")]
public partial class Action_SetNearbyChairHasTarget : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Agent;
    [SerializeReference] public BlackboardVariable<Structure_Chair> Target;

    protected override Status OnStart()
    {
        Structure[] structures = GameManager.instance.structures.ToArray();
        List<Structure_Chair> chairs = new List<Structure_Chair>();

        for (int i = 0; i < structures.Length; i++)
        {
            if (structures[i] is Structure_Chair structure_Chair)
            {
                chairs.Add(structure_Chair);
            }
        }

        if (chairs.Count == 0)
            return Status.Failure;

        Structure_Chair chair = null;
        float minDist = float.MaxValue;
        for (int i = 0; i < chairs.Count; i++)
        {
            if (!chairs[i].ocuppied)
            {
                float newDist = Vector3.Distance(Agent.Value.transform.position, chairs[i].transform.position);
                if (newDist < minDist)
                {
                    minDist = newDist;
                    chair = chairs[i];
                }
            }
        }

        Target.Value = chair;

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

