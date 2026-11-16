using UnityEngine;

public abstract class CharacterState
{
    public string name = "";
    public virtual void Enter(CharacterBehaviour InBehaviour)
    {
        InBehaviour.currentTextState.text = name;
    }
    public virtual void Update(CharacterBehaviour InBehaviour)
    {
    }
    public virtual void Exit(CharacterBehaviour InBehaviour)
    {
    }
}
