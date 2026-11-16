using UnityEngine;
using UnityEngine.AI;

public class CharacterBehaviour_NPC : CharacterBehaviour
{
    [Header("Follow")]
    public float followMaxDistance;
    public float followStoppingDistance;

    void Start()
    {
        ChangeState(new CharacterState_Follow(GameObject.FindGameObjectWithTag("Player").transform, followStoppingDistance, followMaxDistance));
    }
    private void Update()
    {
        _state?.Update(this);
        ApplyAnimation();
    }
}
