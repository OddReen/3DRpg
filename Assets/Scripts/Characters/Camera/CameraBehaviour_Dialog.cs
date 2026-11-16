using Unity.AppUI.UI;
using Unity.Cinemachine;
using UnityEngine;

public class CameraBehaviour_Dialog : CameraBehaviour
{
    private GameObject interactionTarget;

    private CinemachineThirdPersonFollow _cinemachineThirdPersonFollow;
    private CinemachineHardLookAt _cinemachineHardLookAt;

    public CameraBehaviour_Dialog(GameObject InInteractionTarget)
    {
        interactionTarget = InInteractionTarget;
    }
    public override void Enter(CharacterBehaviour_Player InBehaviour)
    {
        base.Enter(InBehaviour);
        _cinemachineCamera.LookAt = interactionTarget.transform;
        _cinemachineCamera.Follow = InBehaviour.transform;

        if (!(_cinemachineThirdPersonFollow = GameManager.instance.camera.GetComponent<CinemachineThirdPersonFollow>()))
        {
            _cinemachineThirdPersonFollow = GameManager.instance.camera.AddComponent<CinemachineThirdPersonFollow>();
        }
        if (!(_cinemachineHardLookAt = GameManager.instance.camera.GetComponent<CinemachineHardLookAt>()))
        {
            _cinemachineHardLookAt = GameManager.instance.camera.AddComponent<CinemachineHardLookAt>();
        }

        _cinemachineThirdPersonFollow.ShoulderOffset = new Vector3(-0.75f, 1.5f, -1);
        _cinemachineHardLookAt.LookAtOffset = new Vector3(0, 1.25f, 0);

    }
    public override void Update(CharacterBehaviour_Player InBehaviour)
    {

    }
    public override void Exit(CharacterBehaviour_Player InBehaviour)
    {
        Object.Destroy(_cinemachineThirdPersonFollow);
        Object.Destroy(_cinemachineHardLookAt);
    }
}
