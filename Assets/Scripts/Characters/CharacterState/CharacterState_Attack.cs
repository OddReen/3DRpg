using UnityEngine;

public class CharacterState_Attack : CharacterState
{
    private GameObject target;

    private float localHitTime;
    private float globalHitTime;

    bool hasHit = false;

    public CharacterState_Attack(GameObject InTarget)
    {
        name = "Attack";
        target = InTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.transform.LookAt(target.transform, Vector3.up);
        InBehaviour.animator.CrossFade("Attack_Sword", .1f);
        InBehaviour.animator.Update(0.0f);

        localHitTime = .50f;
        globalHitTime = Time.time + localHitTime;
    }

    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);

        AnimatorStateInfo stateInfo = InBehaviour.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack_Sword") && stateInfo.normalizedTime >= 1f)
        {
            InBehaviour.ChangeState(new CharacterState_Idle());
        }

        if (Time.time >= globalHitTime && !hasHit)
        {
            hasHit = true;
            target.GetComponent<HealthSystem>().Damage(30.0f);
        }
    }

    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
    }
}
