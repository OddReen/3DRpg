using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CameraBehaviour_Orbit : CameraBehaviour
{
    public Transform orbitTarget;

    [Header("Orbit")]
    private float yaw;
    private float pitch;

    private float currentYaw;
    private float currentPitch;

    private float yawVelocity;
    private float pitchVelocity;

    [Header("Zoom")]
    private float targetDistance;
    private float currentDistance;
    private float distanceVelocity;

    [Header("Focus")]
    public Vector3 focusGroundPosition;
    public bool focusOnNavMesh;

    [Header("Movement")]
    public Vector2 smoothedInput;
    public Vector2 inputVelocity;

    public float currentMoveSpeed;
    public float speedVelocity;

    public override void Enter(CharacterBehaviour_Player InBehaviour)
    {
        base.Enter(InBehaviour);

        if (!(_cinemachineFollow = GameManager.instance.camera.GetComponent<CinemachineFollow>()))
        {
            _cinemachineFollow = GameManager.instance.camera.AddComponent<CinemachineFollow>();
        }
        if (!(_cinemachineRotationComposer = GameManager.instance.camera.GetComponent<CinemachineRotationComposer>()))
        {
            _cinemachineRotationComposer = GameManager.instance.camera.AddComponent<CinemachineRotationComposer>();
        }

        InBehaviour.cameraData.target = InBehaviour.transform;

        var trackerSettings = _cinemachineFollow.TrackerSettings;
        trackerSettings.BindingMode =
            Unity.Cinemachine.TargetTracking.BindingMode.WorldSpace;
        trackerSettings.RotationDamping = Vector3.one * InBehaviour.cameraData.followDamping;
        trackerSettings.PositionDamping = Vector3.one * InBehaviour.cameraData.followDamping;
        _cinemachineFollow.TrackerSettings = trackerSettings;

        _cinemachineRotationComposer.Damping = Vector2.one * InBehaviour.cameraData.aimDamping;

        if (InBehaviour.cameraData.target == null)
        {
            InBehaviour.enabled = false;
            return;
        }

        InBehaviour.cameraData.movementSmoothTime = Mathf.Max(0.01f, InBehaviour.cameraData.movementSmoothTime);
        InBehaviour.cameraData.rotationSmoothTime = Mathf.Max(0.01f, InBehaviour.cameraData.rotationSmoothTime);
        InBehaviour.cameraData.zoomSmoothTime = Mathf.Max(0.01f, InBehaviour.cameraData.zoomSmoothTime);
        InBehaviour.cameraData.distance = Mathf.Clamp(InBehaviour.cameraData.distance, InBehaviour.cameraData.minDistance, InBehaviour.cameraData.maxDistance);
        InBehaviour.cameraData.pitchAngle = Mathf.Clamp(InBehaviour.cameraData.pitchAngle, InBehaviour.cameraData.minPitch, InBehaviour.cameraData.maxPitch);

        yaw = InBehaviour.cameraData.target.eulerAngles.y;
        pitch = InBehaviour.cameraData.pitchAngle;
        targetDistance = InBehaviour.cameraData.distance;

        currentYaw = yaw;
        currentPitch = pitch;
        currentDistance = targetDistance;
        currentMoveSpeed = InBehaviour.cameraData.moveSpeed;

        if (NavMesh.SamplePosition(InBehaviour.cameraData.target.position, out NavMeshHit startHit, 5f, InBehaviour.cameraData.navMeshAreaMask))
        {
            focusGroundPosition = startHit.position;
            focusOnNavMesh = true;
        }
        else
        {
            focusGroundPosition = InBehaviour.cameraData.target.position;
            focusOnNavMesh = false;
        }

        orbitTarget = GameManager.instance.rotationHighlight.transform;
        _cinemachineCamera.Follow = orbitTarget;
        _cinemachineCamera.LookAt = orbitTarget;
    }
    public override void Update(CharacterBehaviour_Player InBehaviour)
    {
        base.Update(InBehaviour);
        HandleOrbit(InBehaviour);
        HandleZoom(InBehaviour);
        Vector3 relativeOffset = (InBehaviour.transform.right * InBehaviour.cameraData.localOffset.x) + (InBehaviour.transform.up * InBehaviour.cameraData.localOffset.y) + (InBehaviour.transform.forward * InBehaviour.cameraData.localOffset.z);
        focusGroundPosition = InBehaviour.transform.position + relativeOffset;
    }
    public override void LateUpdate(CharacterBehaviour_Player InBehaviour)
    {
        if (orbitTarget == null)
            return;

        float deltaTime = Time.deltaTime;

        // Smooth rotation
        currentYaw = Mathf.SmoothDampAngle(
            currentYaw, yaw, ref yawVelocity,
            InBehaviour.cameraData.rotationSmoothTime,
            Mathf.Infinity, deltaTime);

        currentPitch = Mathf.SmoothDamp(
            currentPitch, pitch, ref pitchVelocity,
            InBehaviour.cameraData.rotationSmoothTime,
            Mathf.Infinity, deltaTime);

        // Smooth zoom
        currentDistance = Mathf.SmoothDamp(
            currentDistance, targetDistance, ref distanceVelocity,
            InBehaviour.cameraData.zoomSmoothTime,
            Mathf.Infinity, deltaTime);

        // Update pivot position
        orbitTarget.position = focusGroundPosition + InBehaviour.cameraData.focusOffset;

        // Calculate camera offset
        float pitchRadians = currentPitch * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            0f,
            Mathf.Sin(pitchRadians) * currentDistance,
            -Mathf.Cos(pitchRadians) * currentDistance
        );

        // Apply yaw directly to the offset
        _cinemachineFollow.FollowOffset =
            Quaternion.Euler(0f, currentYaw, 0f) * offset;
    }
    public override void Exit(CharacterBehaviour_Player InBehaviour)
    {
        base.Exit(InBehaviour);

        Object.Destroy(_cinemachineFollow);
        Object.Destroy(_cinemachineRotationComposer);
    }

    public virtual void HandleOrbit(CharacterBehaviour_Player InBehaviour)
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            return;
        }

        if (mouse.rightButton.isPressed || mouse.middleButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Vector2 delta = mouse.delta.ReadValue();

            yaw += delta.x * InBehaviour.cameraData.rotationSensitivity;

            pitch -= delta.y * InBehaviour.cameraData.rotationSensitivity;
            pitch = Mathf.Clamp(pitch, InBehaviour.cameraData.minPitch, InBehaviour.cameraData.maxPitch);

            orbitTarget.gameObject.SetActive(true);
        }
        else
        {
            orbitTarget.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
    public virtual void HandleZoom(CharacterBehaviour_Player InBehaviour)
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
            return;

        float scroll = mouse.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 1f)
            scroll /= 120f;

        targetDistance -= scroll * InBehaviour.cameraData.zoomSpeed;

        targetDistance = Mathf.Clamp(targetDistance, InBehaviour.cameraData.minDistance, InBehaviour.cameraData.maxDistance);
    }
    public virtual void HandleMovement(CharacterBehaviour_Player InBehaviour)
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        Vector2 input = Vector2.zero;
        if (keyboard.wKey.isPressed) input.y += 1f;
        if (keyboard.sKey.isPressed) input.y -= 1f;
        if (keyboard.dKey.isPressed) input.x += 1f;
        if (keyboard.aKey.isPressed) input.x -= 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        smoothedInput = Vector2.SmoothDamp(smoothedInput, input, ref inputVelocity, InBehaviour.cameraData.movementSmoothTime);

        bool sprinting = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

        float desiredSpeed = InBehaviour.cameraData.moveSpeed * (sprinting ? InBehaviour.cameraData.sprintMultiplier : 1f);

        currentMoveSpeed = Mathf.SmoothDamp(currentMoveSpeed, desiredSpeed, ref speedVelocity, InBehaviour.cameraData.movementSmoothTime);

        if (smoothedInput.sqrMagnitude < 0.000001f)
            return;

        Quaternion yawRotation = Quaternion.Euler(0f, currentYaw, 0f);

        Vector3 forward = yawRotation * Vector3.forward;
        Vector3 right = yawRotation * Vector3.right;

        Vector3 movement = (forward * smoothedInput.y + right * smoothedInput.x) * currentMoveSpeed * Time.deltaTime;

        if (!focusOnNavMesh)
        {
            if (!NavMesh.SamplePosition(focusGroundPosition, out NavMeshHit recoveryHit, 5f, InBehaviour.cameraData.navMeshAreaMask))
            {
                return;
            }

            focusGroundPosition = recoveryHit.position;
            focusOnNavMesh = true;
        }

        Vector3 proposedPosition = focusGroundPosition + movement;

        if (!NavMesh.SamplePosition(proposedPosition, out NavMeshHit navHit, InBehaviour.cameraData.navMeshSampleDistance, InBehaviour.cameraData.navMeshAreaMask))
        {
            return;
        }

        if (NavMesh.Raycast(focusGroundPosition, navHit.position, out NavMeshHit edgeHit, InBehaviour.cameraData.navMeshAreaMask))
        {
            return;
        }

        focusGroundPosition = navHit.position;
    }
}