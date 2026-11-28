using UnityEngine;

public class CharacterState_MoveToTarget : CharacterState
{
    private Transform target;
    private CharacterState behaviourOnArrival;
    private float stoppingDistance;

    public CharacterState_MoveToTarget(Transform InTarget, float InStoppingDistance, CharacterState InBehaviourOnArrival)
    {
        name = "MoveToTarget";
        target = InTarget;
        stoppingDistance = InStoppingDistance;
        behaviourOnArrival = InBehaviourOnArrival;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.PlayMovementStance();
        InBehaviour.agent.stoppingDistance = stoppingDistance;
        InBehaviour.agent.SetDestination(target.position);
    }

    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);

        if (target == null)
            return;

        InBehaviour.RotateToMoveDirectionSmoothed(InBehaviour.agent.velocity.normalized);

        InBehaviour.agent.SetDestination(target.position);

        if (InBehaviour.HasArrived())
        {
            InBehaviour.ChangeState(behaviourOnArrival);
        }
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
    }
}