using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Normal Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    public AnimationClip AttackAnimation;

    public float AttackLength()
    {
        return AttackAnimation.length;
    }
}
