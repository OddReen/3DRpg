using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController_WASD : PlayerController
{
    public NavMeshAgent agent;
    public Transform movementCamera;

    [Header("Movement")]
    [SerializeField] private float destinationDistance = 1f;
    [SerializeField] private float refreshDistance = 2f;
    [SerializeField] private float directionChangeThreshold = 10f;

    public override void Enter(CharacterBehaviour_Player InBehaviour)
    {
        if (agent == null)
            agent = InBehaviour.GetComponent<NavMeshAgent>();

        if (movementCamera == null && Camera.main != null)
            movementCamera = Camera.main.transform;

        agent.updateRotation = true;
    }

    public override void Update(CharacterBehaviour_Player InBehaviour)
    {
        if (Keyboard.current == null || !agent.isOnNavMesh)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;

        InBehaviour.isSprinting = Keyboard.current.shiftKey.isPressed;

        if (input.sqrMagnitude > 1f)
            input.Normalize();

        Vector3 forward = movementCamera != null ? movementCamera.forward : InBehaviour.transform.forward;
        forward.y = 0f;
        forward.Normalize();
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 direction = (forward * input.y + right * input.x).normalized;
        Vector3 velocity = direction * agent.speed;

        if (InBehaviour.isSprinting)
        {
            velocity *= 2.0f;
        }
        agent.Move(velocity * Time.deltaTime);

        InBehaviour.RotateToMoveDirectionSmoothed(direction);
        InBehaviour.ApplyAnimation(velocity);
    }

    public override void Exit(CharacterBehaviour_Player InBehaviour)
    {
    }
}