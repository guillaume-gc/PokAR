using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WishAttackAnimation : PokemonAnimationSkeleton
{
    public WishAttackAnimation() : base()
    {
        name = "attack3";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;
        attackTarget = AttackTarget.MYSELF;

        Init();
    }
}
