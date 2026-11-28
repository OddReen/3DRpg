using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Structure : InteractionObject
{
    [SerializeField] public List<MeshRenderer> defaultVisual;
    [SerializeField] public List<MeshRenderer> cuttoutVisual;

    public virtual void ExecuteAction(CharacterBehaviour InBehaviour)
    {

    }

    public void SetVisual(bool InCuttout)
    {
        if (InCuttout)
        {
            foreach (var item in defaultVisual)
            {
                item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
            foreach (var item in cuttoutVisual)
            {
                item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
        else
        {
            foreach (var item in defaultVisual)
            {
                item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
            foreach (var item in cuttoutVisual)
            {
                item.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
    }
}
