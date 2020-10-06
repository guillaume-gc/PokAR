using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAttackHurt : PokemonAnimationSkeleton
{
    public PhysicalAttackHurt() : base()
    {
        name = "hurt0";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;

        attackTarget = AttackTarget.ADVERSARY;

        Init();
    }
}
