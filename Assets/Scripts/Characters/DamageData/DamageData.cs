using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "Scriptable Objects/DamageData")]
[Serializable]
public class DamageData : ScriptableObject
{
    public string name = "";
    public float bloodLossAmount;
}
