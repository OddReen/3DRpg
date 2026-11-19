using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SocialButton
{
    public SocialButton(GameObject InUIButton, CharacterState InCharacterState)
    {
        UIButton = InUIButton;
        characterState = InCharacterState;
    }
    public GameObject UIButton;
    public CharacterState characterState;
}
public class CharacterState_Interaction : CharacterState
{
    private GameObject interactionTarget;

    public List<SocialButton> socialButtons = new List<SocialButton>();

    public CharacterState_Interaction(GameObject InInteractionTarget)
    {
        name = "Interaction";
        interactionTarget = InInteractionTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);

        List<CharacterState> buttonStates = new List<CharacterState>
        {
            new CharacterState_Follow(InBehaviour.gameObject.transform, 2.0f, 4.0f)
        };

        CharacterBehaviour_Player playerBehaviour = (CharacterBehaviour_Player)InBehaviour;
        if (playerBehaviour != null)
        {
            playerBehaviour.ChangeController(null);

            interactionTarget.GetComponent<CharacterBehaviour_NPC>().animator.Play("Interaction");
            playerBehaviour.socialOptions.SetActive(true);
            foreach (CharacterState buttonState in buttonStates)
            {
                GameObject newButtonUI = GameObject.Instantiate(playerBehaviour.socialOptionPref, playerBehaviour.socialOptionsFrontPanel.transform);
                newButtonUI.GetComponentInChildren<TextMeshProUGUI>().text = buttonState.name;
                newButtonUI.GetComponent<Button>().onClick.AddListener(() => interactionTarget.GetComponent<CharacterBehaviour_NPC>().ChangeState(buttonState));
                newButtonUI.GetComponent<Button>().onClick.AddListener(() => InBehaviour.ChangeState(new CharacterState_Idle()));

                SocialButton newSocialButton = new SocialButton(newButtonUI, buttonState);
                socialButtons.Add(newSocialButton);
            }
        }

        InBehaviour.RotateToMoveDirection();
        interactionTarget.transform.LookAt(InBehaviour.transform);
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);

        foreach (var item in socialButtons)
        {
            GameObject.Destroy(item.UIButton);
        }
        socialButtons.Clear();

        CharacterBehaviour_Player playerBehaviour = (CharacterBehaviour_Player)InBehaviour;
        playerBehaviour.ChangeController(new PlayerController_Mouse());
        playerBehaviour.socialOptions.SetActive(false);
    }
}