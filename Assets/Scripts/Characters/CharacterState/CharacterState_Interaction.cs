using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterState_Interaction : CharacterState
{
    private GameObject interactionTarget;

    public CharacterState_Interaction(GameObject InInteractionTarget)
    {
        name = "Interaction";
        interactionTarget = InInteractionTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);

        CharacterBehaviour_Player playerBehaviour = (CharacterBehaviour_Player)InBehaviour;
        if (playerBehaviour != null)
        {
            interactionTarget.GetComponent<CharacterBehaviour_NPC>().animator.Play("Interaction");
            playerBehaviour.dialogUI.SetActive(true);
            //playerBehaviour.ChangeCamera(new CameraBehaviour_Dialog(interactionTarget));
        }

        InBehaviour.RotateToMoveDirection();
        interactionTarget.transform.LookAt(InBehaviour.transform);
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            InBehaviour.ChangeState(new CharacterState_Idle());
        }
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
        CharacterBehaviour_Player playerBehaviour = (CharacterBehaviour_Player)InBehaviour;
        playerBehaviour.dialogUI.SetActive(false);
    }
}