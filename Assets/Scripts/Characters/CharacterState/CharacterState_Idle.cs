using UnityEngine;

public class CharacterState_Idle : CharacterState
{
    public CharacterState_Idle()
    {
        name = "Idle";
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.PlayMovementStance();
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
    }
}
