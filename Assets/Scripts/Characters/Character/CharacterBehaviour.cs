using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[Serializable] public class Inventory
{
    [SerializeField] public GameObject rightWeapon;

    List<GameObject> items;
}
[Serializable] public class CharacterSlots
{
    [SerializeField] public Transform leftHandSlot;
    [SerializeField] public Transform rightHandSlot;
    [SerializeField] public Transform holsterSlot;

    [SerializeField] public GameObject head;
    [SerializeField] public GameObject torso;
    [SerializeField] public GameObject arms;
    [SerializeField] public GameObject legs;
    [SerializeField] public GameObject feet;
}
public abstract class CharacterBehaviour : InteractionObject
{
    [Header("Character Data")]
    [SerializeField] public CharacterSlots characterSlots;
    [SerializeField] public Inventory inventory;

    [Header("References")]
    public Animator animator;
    public NavMeshAgent agent;
    public TextMeshProUGUI currentTextState;

    [Header("Movement")]
    public float walkSpeed = 1f;
    public float runSpeed = 1f;
    public float speedMultiplier = 1f;
    public float animationSmoothSpeed = 1f;
    public bool isSprinting = false;
    private Vector3 currentVelocity;
    public int movementStanceIndex = 0;
    public float rotationSpeed = 2.5f;

    [Header("State")]
    public CharacterState _state;
    public GameObject interactionTarget;

    [Header("Holster")]
    public bool hasWeapon = false;
    public bool isWeaponHolstered = false;

    public void ChangeMovementStance(int InIndex)
    {
        movementStanceIndex = InIndex;
    }
    public void PlayMovementStance()
    {
        if (movementStanceIndex == 0)
        {
            animator.CrossFade("Movement", .1f);
        }
        else if (movementStanceIndex == 1)
        {
            animator.CrossFade("Movement_Weapon", .1f);
        }
    }
    public void ApplyAnimation(Vector3 InVelocity)
    {
        Vector3 velocityTarget = transform.InverseTransformDirection(InVelocity / agent.speed);
        if (isSprinting)
        {
            velocityTarget *= 2;
        }
        currentVelocity = Vector3.MoveTowards(currentVelocity, velocityTarget, Time.deltaTime * animationSmoothSpeed);

        animator.SetFloat("VelocityY", currentVelocity.z);
        animator.SetFloat("VelocityX", currentVelocity.x);

        animator.SetFloat("VelocityMultiplier", Mathf.Max(1.0f, currentVelocity.magnitude * speedMultiplier));

    }
    public bool HasArrived()
    {
        if (agent.pathPending)
            return false;

        return agent.remainingDistance <= agent.stoppingDistance;
    }
    public void RotateToMoveDirection()
    {
        Vector3 lookAt = agent.desiredVelocity.normalized;
        lookAt = new Vector3(lookAt.x, 0f, lookAt.z).normalized;
        gameObject.transform.LookAt(transform.position + lookAt);
    }
    public void RotateToMoveDirectionSmoothed(Vector3 InDirection)
    {
        Vector3 lookAtTarget = new Vector3(InDirection.x, 0f, InDirection.z).normalized;
        Vector3 lookAt = Vector3.RotateTowards(agent.transform.forward, lookAtTarget, Time.deltaTime * rotationSpeed, 0.0f); 

        gameObject.transform.LookAt(transform.position + lookAt);
    }
    public void ChangeState(CharacterState InState)
    {
        _state?.Exit(this);
        _state = InState;
        _state?.Enter(this);
    }

}