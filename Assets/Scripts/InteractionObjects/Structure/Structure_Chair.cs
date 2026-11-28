using UnityEngine;

public class Structure_Chair : Structure
{
    public bool ocuppied = false;
    public override void ExecuteAction(CharacterBehaviour InBehaviour)
    {
        InBehaviour.ChangeState(new CharacterState_MoveToTarget(transform, 1.0f, new CharacterState_SitInChair(this)));
    }
}
