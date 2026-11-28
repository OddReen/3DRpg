using UnityEngine;

public class CharacterBehaviour_NPC : CharacterBehaviour
{
    [Header("Follow")]
    public float followMaxDistance;
    public float followStoppingDistance;

    void Start()
    {
        //ChangeState(new CharacterState_MoveToTarget(GameObject.FindGameObjectWithTag("Player").transform, 1.0f, new CharacterState_Attack(GameObject.FindGameObjectWithTag("Player"))));
    }
    private void Update()
    {
        //_state?.Update(this);
        ApplyAnimation(agent.velocity);
    }
}
