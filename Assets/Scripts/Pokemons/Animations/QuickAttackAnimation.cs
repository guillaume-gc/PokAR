using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAttackAnimation : PokemonAnimationSkeleton
{
    public QuickAttackAnimation() : base()
    {
        name = "attack0";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;
        attackTarget = AttackTarget.MYSELF;

        Init();
    }
}
