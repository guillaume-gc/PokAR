using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttackHurt : PokemonAnimationSkeleton
{
    public ThunderAttackHurt() : base()
    {
        name = "hurt2";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;

        attackTarget = AttackTarget.ADVERSARY;

        Init();
    }
}
