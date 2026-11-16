using UnityEngine;

public class CharacterState_SitInChair : CharacterState
{
    GameObject target;

    public CharacterState_SitInChair(GameObject InTarget)
    {
        name = "SitInChair";
        target = InTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);
        InBehaviour.agent.enabled = false;
        InBehaviour.animator.Play("SatInChair");

        Vector3 rotate = new Vector3(0.0f, target.transform.rotation.eulerAngles.y, 0.0f);
        InBehaviour.transform.SetPositionAndRotation(target.transform.position, Quaternion.Euler(rotate));
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        InBehaviour.agent.enabled = true;
        base.Enter(InBehaviour);
    }
}