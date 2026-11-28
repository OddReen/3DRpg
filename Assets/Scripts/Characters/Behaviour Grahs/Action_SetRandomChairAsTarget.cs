using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Action_SetRandomChairAsTarget", story: "Set Random Chair As Target", category: "Action", id: "8807952ec0eaf05b8f6844d22422445d")]
public partial class Action_SetRandomChairAsTarget : Action
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

        List<Structure_Chair> unocuppiedChairs = new List<Structure_Chair>();
        for (int i = 0; i < chairs.Count; i++)
        {
            if (!chairs[i].ocuppied)
            {
                unocuppiedChairs.Add(chairs[i]);
            }
        }

        if (unocuppiedChairs.Count == 0)
            return Status.Failure;

        Target.Value = unocuppiedChairs[Random.Range(0, unocuppiedChairs.Count)];

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

