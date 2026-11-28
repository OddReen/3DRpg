using UnityEngine;

public class CharacterState_Follow : CharacterState
{
    private Transform target;
    private float stoppingDistance;
    private float maxDistance;

    public CharacterState_Follow(Transform InTarget, float InStoppingDistance, float InMaxDistance)
    {
        name = "Follow";
        target = InTarget;
        stoppingDistance = InStoppingDistance;
        maxDistance = InMaxDistance;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.PlayMovementStance();
        InBehaviour.agent.stoppingDistance = stoppingDistance;
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
        InBehaviour.RotateToMoveDirectionSmoothed(InBehaviour.agent.velocity.normalized);
        if (Vector3.Distance(InBehaviour.transform.position, target.position) > maxDistance)
        {
            InBehaviour.agent.SetDestination(target.position);
        }
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
    }

    bool HasArrived(CharacterBehaviour InBehaviour)
    {
        if (InBehaviour.agent.pathPending)
            return false;

        return InBehaviour.agent.remainingDistance <= InBehaviour.agent.stoppingDistance;
    }

}
