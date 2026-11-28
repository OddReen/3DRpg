using UnityEngine;

public class CharacterState_SitInChair : CharacterState
{
    Structure_Chair target;

    public CharacterState_SitInChair(Structure_Chair InTarget)
    {
        name = "SitInChair";
        target = InTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);

        target.ocuppied = true;
        InBehaviour.agent.enabled = false;
        InBehaviour.animator.CrossFade("SatInChair", .1f);

        Vector3 rotate = new Vector3(0.0f, target.transform.rotation.eulerAngles.y, 0.0f);
        InBehaviour.transform.SetPositionAndRotation(target.transform.position, Quaternion.Euler(rotate));
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        target.ocuppied = false;
        InBehaviour.agent.enabled = true;
    }
}