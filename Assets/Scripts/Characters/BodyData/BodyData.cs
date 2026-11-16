using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LimbData
{
    public string name = "";
    public float currentHitPoints = 100.0f;
    public float maxHitPoints = 100.0f;
    public List<DamageData> damageList = new List<DamageData>();
}

[CreateAssetMenu(fileName = "BodyData", menuName = "Scriptable Objects/BodyData")]
[Serializable]
public class BodyData : ScriptableObject
{
    [SerializeField] public LimbData head;
    [SerializeField] public LimbData torso;
    [SerializeField] public LimbData rightArm;
    [SerializeField] public LimbData leftArm;
    [SerializeField] public LimbData rightLeg;
    [SerializeField] public LimbData leftLeg;

    BodyData(LimbData head, LimbData torso, LimbData rightArm, LimbData leftArm, LimbData rightLeg, LimbData leftLeg)
    {
        this.head = head;
        this.torso = torso;
        this.rightArm = rightArm;
        this.leftArm = leftArm;
        this.rightLeg = rightLeg;
        this.leftLeg = leftLeg;
    }
}
