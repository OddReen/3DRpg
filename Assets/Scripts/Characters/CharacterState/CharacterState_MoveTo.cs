using UnityEngine;

public class CharacterState_MoveTo : CharacterState
{
    private CharacterState behaviourOnArrival;
    private Vector3 destination;
    private float stoppingDistance;

    public CharacterState_MoveTo(Vector3 InDestination, float InStoppingDistance, CharacterState InBehaviourOnArrival)
    {
        name = "MoveTo";
        behaviourOnArrival = InBehaviourOnArrival;
        destination = InDestination;
        stoppingDistance = InStoppingDistance;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.PlayMovementStance();
        InBehaviour.agent.stoppingDistance = stoppingDistance;
        InBehaviour.agent.SetDestination(destination);
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);

        InBehaviour.RotateToMoveDirection();

        if (InBehaviour.HasArrived())
        {
            InBehaviour.ChangeState(behaviourOnArrival);
        }
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
    }
}
