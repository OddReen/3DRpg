using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    CharacterBehaviour characterBehaviour;

    public float currentBlood = 100.0f;
    public float maxBlood = 100.0f;

    [SerializeField] public BodyData bodyData;

    void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
    }
    public void Damage(float InAmount)
    {
        currentBlood -= InAmount;
        if (currentBlood <= 0.0f)
        {
            characterBehaviour.animator.Play("Die");
            return;
        }
        characterBehaviour.animator.Play("TakeDamage");
    }
}
