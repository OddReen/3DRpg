using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "Attack Target", category: "Action", id: "01ef1c44d4bab9f61592842e3bbca19f")]
public partial class Action_AttackTarget : Action
{
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBehaviour> Target;
    [SerializeReference] public BlackboardVariable<float> localHitTime;
    [SerializeReference] public float globalHitTime;
    [SerializeReference] public float animationDuration;
    [SerializeReference] public bool hasHit;

    protected override Status OnStart()
    {
        Agent.Value.transform.LookAt(Target.Value.transform, Vector3.up);
        Agent.Value.animator.CrossFade("Attack_Sword", .1f);
        Agent.Value.animator.Update(0.0f);

        AnimatorStateInfo stateInfo = Agent.Value.animator.GetCurrentAnimatorStateInfo(0);

        animationDuration = Time.time + stateInfo.length;
        globalHitTime = Time.time + localHitTime;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        AnimatorStateInfo stateInfo = Agent.Value.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack_Sword") && stateInfo.normalizedTime >= 1f)
        {
            Agent.Value.ChangeState(new CharacterState_Idle());
        }

        if (Time.time >= globalHitTime && !hasHit)
        {
            hasHit = true;
            Target.Value.GetComponent<HealthSystem>().Damage(30.0f);
        }
        if (Time.time >= animationDuration)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
        hasHit = false;
        base.OnEnd();
    }
}

