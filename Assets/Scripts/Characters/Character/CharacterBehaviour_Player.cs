using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[Serializable]
public class CameraData
{
    [SerializeField] public GameObject hoverHighlight;

    [Header("Starting Position")]
    [SerializeField] public Transform target;
    [SerializeField] public Vector3 focusOffset = new Vector3(0f, 1.2f, 0f);

    [Header("Movement Smoothing")]
    [SerializeField] public float movementSmoothTime = 0.12f;

    [Header("Camera Movement")]
    [SerializeField] public float moveSpeed = 8f;
    [SerializeField] public float sprintMultiplier = 2f;
    [SerializeField] public float navMeshSampleDistance = 1f;
    [SerializeField] public int navMeshAreaMask = NavMesh.AllAreas;

    [Header("Orbit")]
    [SerializeField] public Vector3 localOffset;
    [SerializeField] public float distance = 10f;
    [SerializeField] public float pitchAngle = 35f;
    [SerializeField] public float minPitch = 15f;
    [SerializeField] public float maxPitch = 75f;
    [SerializeField] public float rotationSensitivity = 0.2f;

    [SerializeField] public float rotationSmoothTime = 0.12f;
    [SerializeField] public float followDamping = 0.2f;
    [SerializeField] public float aimDamping = 0.15f;

    [Header("Zoom")]
    [SerializeField] public float zoomSpeed = 1.5f;
    [SerializeField] public float minDistance = 3f;
    [SerializeField] public float maxDistance = 20f;
    [SerializeField] public float zoomSmoothTime = 0.15f;
}
public class CharacterBehaviour_Player : CharacterBehaviour
{
    [Header("Behaviour")]
    public PlayerController playerController;
    public CameraBehaviour cameraBehaviour;

    [Header("References")]
    public LayerMask groundMask;
    public float rayDistance = 500f;
    public float navMeshSampleDistance = 2f;

    [Header("Camera")]
    public bool isThirdPerson = false;

    [Header("ClickToMove")]
    public float lastTimeClicked = 0;
    public float doubleClickTimer = .25f;

    [SerializeField] public CameraData cameraData;
    [SerializeField] public GameObject socialOptions;
    [SerializeField] public GameObject socialOptionsFrontPanel;
    [SerializeField] public GameObject socialOptionPref;

    void Start()
    {
        isThirdPerson = false;
        ChangeController(new PlayerController_Mouse());
        ChangeCamera(new CameraBehaviour_Orbit());
    }
    void Update()
    {
        //SwitchController();
        DrawWeapon();
        TriggerAttack();
        playerController?.Update(this);
        cameraBehaviour?.Update(this);
        _state?.Update(this);
        ApplyAnimation(agent.velocity);
    }
    private void LateUpdate()
    {
        cameraBehaviour?.LateUpdate(this);
    }

    public void DrawWeapon()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (hasWeapon)
            {
                ChangeState(new CharacterState_Holster(inventory.rightWeapon));
            }
        }
    }

    public void SwitchController()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            if (isThirdPerson)
            {
                isThirdPerson = false;
                ChangeController(new PlayerController_WASD());
                ChangeCamera(new CameraBehaviour_Orbit());
            }
            else
            {
                isThirdPerson = true;
                ChangeController(new PlayerController_Mouse());
            }
        }
    }

    public void TriggerAttack()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            ChangeState(new CharacterState_MoveToTarget(GameObject.FindGameObjectWithTag("NPC").transform, 1.0f, new CharacterState_Attack(GameObject.FindGameObjectWithTag("NPC"))));
        }
    }

    public void ChangeController(PlayerController InController)
    {
        playerController?.Exit(this);
        playerController = InController;
        playerController?.Enter(this);
    }
    public void ChangeCamera(CameraBehaviour InController)
    {
        cameraBehaviour?.Exit(this);
        cameraBehaviour = InController;
        cameraBehaviour?.Enter(this);
    }
}
