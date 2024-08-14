using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float glowDuration = 0.5f;
    int glowAmountShader = Shader.PropertyToID("_GlowEffectAmount");
    float glowAmount;
    public void GlowStart(Material starObj)
    {
        glowAmount = starObj.GetFloat("_GlowEffectAmount");
        DOTween.To(GetGlow, SetGlow, 15f, (glowAmount/15)*glowDuration).OnUpdate(() => Glowing(starObj)).OnComplete(() => GlowComplete(starObj)).SetDelay(Random.Range(2f, 7f));
    }

    float GetGlow()
    {
        return glowAmount;
    }

    void SetGlow(float _glowAmount)
    {
        glowAmount = _glowAmount;
    }

    void Glowing(Material starObj)
    {
        starObj.SetFloat("_GlowEffectAmount", GetGlow());
    }

    void GlowComplete(Material starObj)
    {
        DOTween.To(GetGlow, SetGlow, 5f, glowDuration).OnUpdate(() => Glowing(starObj)).OnComplete(() => GlowStart(starObj));
    }

}
