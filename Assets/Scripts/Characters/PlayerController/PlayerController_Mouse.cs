using Unity.AppUI.Navigation;
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
            if (hit.collider.TryGetComponent<InteractionObject>(out InteractionObject hitBehaviour))
            {
                GameManager.instance.hoverHighlight.transform.position = hit.collider.transform.position + Vector3.up * .1f;

                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GameManager.instance.destinationHighlight.transform.position = hitBehaviour.transform.position + Vector3.up * .1f;
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

                    if (hitBehaviour is CharacterBehaviour_NPC characterBehaviour_NPC)
                    {
                        if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
                        {
                            InBehaviour.ChangeState(new CharacterState_MoveToTarget(hit.collider.gameObject.transform, 1.0f, new CharacterState_Interaction(hit.collider.gameObject)));
                        }
                    }
                    else if (hitBehaviour is Structure structure)
                    {
                        structure.ExecuteAction(InBehaviour);
                    }
                    else if (hitBehaviour is Item item)
                    {
                        InBehaviour.ChangeState(new CharacterState_MoveToTarget(item.transform, 1.0f, new CharacterState_Holster(item.gameObject)));
                    }
                    else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
                    {
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
            else if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, InBehaviour.navMeshSampleDistance, InBehaviour.agent.areaMask))
            {
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GameManager.instance.destinationHighlight.transform.position = navHit.position + Vector3.up * .1f;
                    if (InBehaviour._state != null)
                    {
                        InBehaviour.ChangeState(new CharacterState_Idle());
                    }
                    InBehaviour.ChangeState(new CharacterState_MoveTo(navHit.position, 0.0f, new CharacterState_Idle()));
                }
                else if (Mouse.current != null)
                {
                    GameManager.instance.hoverHighlight.transform.position = navHit.position + Vector3.up * .1f;
                }
            }
        }
    }

    public override void Exit(CharacterBehaviour_Player InBehaviour)
    {

    }
}