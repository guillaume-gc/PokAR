using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttackAnimation : PokemonAnimationSkeleton
{
    public ThunderAttackAnimation() : base()
    {
        name = "attack2";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;
        attackTarget = AttackTarget.MYSELF;

        Init();
    }
}
