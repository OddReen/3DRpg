using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController_Mouse : PlayerController
{
    public override void Enter(CharacterBehaviour_Player InBehaviour)
    {

    }

    public override void Update(CharacterBehaviour_Player InBehaviour)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, InBehaviour.rayDistance, InBehaviour.groundMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.CompareTag("NPC"))
            {
                GameManager.instance.hoverHighlight.transform.position = hit.collider.transform.position + Vector3.up * .1f;

            }
            else if (hit.collider.gameObject.CompareTag("Chair"))
            {
                GameManager.instance.hoverHighlight.transform.position = hit.collider.transform.position + Vector3.up * .1f;
            }
            else if (hit.collider.gameObject.CompareTag("Item"))
            {
                GameManager.instance.hoverHighlight.transform.position = hit.collider.transform.position + Vector3.up * .1f;
            }
            else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
            {
                GameManager.instance.hoverHighlight.transform.position = hit.point;
            }

            // Mouse Pressed
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Time.time < (InBehaviour.lastTimeClicked + InBehaviour.doubleClickTimer))
                {
                    InBehaviour.agent.speed = InBehaviour.runSpeed * InBehaviour.speedMultiplier;
                    InBehaviour.isSprinting = true;
                }
                else
                {
                    InBehaviour.agent.speed = InBehaviour.walkSpeed * InBehaviour.speedMultiplier;
                    InBehaviour.isSprinting = false;
                }
                InBehaviour.lastTimeClicked = Time.time;

                if (hit.collider.gameObject.CompareTag("NPC"))
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
                    {
                        InBehaviour.ChangeState(new CharacterState_MoveToTarget(hit.collider.gameObject.transform, 1.0f, new CharacterState_Interaction(hit.collider.gameObject)));
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Chair"))
                {
                    InBehaviour.ChangeState(new CharacterState_MoveToTarget(hit.collider.gameObject.transform, 1.0f, new CharacterState_SitInChair(hit.collider.gameObject)));
                }
                else if (hit.collider.gameObject.CompareTag("Item"))
                {
                    InBehaviour.ChangeState(new CharacterState_MoveToTarget(hit.collider.gameObject.transform, 1.0f, new CharacterState_Holster(hit.collider.gameObject)));
                }
                else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
                {
                    if (!(InBehaviour.cameraBehaviour is CameraBehaviour_Orbit))
                    {
                        InBehaviour.ChangeCamera(new CameraBehaviour_Orbit());
                    }
                    if (InBehaviour._state != null)
                    {
                        InBehaviour.ChangeState(new CharacterState_Idle());
                    }
                    InBehaviour.ChangeState(new CharacterState_MoveTo(navHit.position, 0.0f, new CharacterState_Idle()));
                }
            }

            // Mouse Pressing
            //else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            //{
            //    InBehaviour.agent.stoppingDistance = 0;
            //    if (hit.collider.gameObject.CompareTag("NPC"))
            //    {
            //        //if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
            //        //{
            //        //    InBehaviour.agent.SetDestination(navHit.position);
            //        //}
            //    }
            //    else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
            //    {
            //        InBehaviour.agent.SetDestination(navHit.position);
            //    }
            //}
        }
    }

    public override void Exit(CharacterBehaviour_Player InBehaviour)
    {

    }
}