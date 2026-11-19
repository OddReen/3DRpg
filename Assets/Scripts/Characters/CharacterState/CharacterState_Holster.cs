using UnityEngine;

public class CharacterState_Holster : CharacterState
{
    private GameObject target;

    public CharacterState_Holster(GameObject InTarget)
    {
        name = "Holster";
        target = InTarget;
    }

    public override void Enter(CharacterBehaviour InBehaviour)
    {
        base.Enter(InBehaviour);

        InBehaviour.hasWeapon = true;
        InBehaviour.inventory.rightWeapon = target;

        Transform slot;
        if (InBehaviour.isWeaponHolstered)
        {
            InBehaviour.ChangeMovementStance(1);
            slot = InBehaviour.characterSlots.rightHandSlot;
        }
        else
        {
            InBehaviour.ChangeMovementStance(0);
            slot = InBehaviour.characterSlots.holsterSlot;
        }
        InBehaviour.isWeaponHolstered = !InBehaviour.isWeaponHolstered;

        InBehaviour.PlayMovementStance();

        target.transform.SetParent(slot);
        target.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }
    public override void Update(CharacterBehaviour InBehaviour)
    {
        base.Update(InBehaviour);
    }
    public override void Exit(CharacterBehaviour InBehaviour)
    {
        base.Exit(InBehaviour);
    }
}