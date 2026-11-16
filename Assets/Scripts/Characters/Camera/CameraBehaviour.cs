
using Unity.Cinemachine;
using UnityEngine;

public class CameraBehaviour
{
    [Header("References")]
    public CinemachineCamera _cinemachineCamera;
    public CinemachineFollow _cinemachineFollow;
    public CinemachineRotationComposer _cinemachineRotationComposer;

    public virtual void Enter(CharacterBehaviour_Player InBehaviour)
    {
        _cinemachineCamera = GameManager.instance.camera.GetComponent<CinemachineCamera>();
    }
    public virtual void Update(CharacterBehaviour_Player InBehaviour) { }
    public virtual void LateUpdate(CharacterBehaviour_Player InBehaviour) { }
    public virtual void Exit(CharacterBehaviour_Player InBehaviour) { }
    public virtual void FollowPlayer(CharacterBehaviour_Player InBehaviour) { }
}