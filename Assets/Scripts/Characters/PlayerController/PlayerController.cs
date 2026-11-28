using UnityEngine;

public abstract class PlayerController
{
    public PlayerController() { }
    public virtual void Enter(CharacterBehaviour_Player InBehaviour) { }
    public virtual void Update(CharacterBehaviour_Player InBehaviour) { }
    public virtual void Exit(CharacterBehaviour_Player InBehaviour) { }
}