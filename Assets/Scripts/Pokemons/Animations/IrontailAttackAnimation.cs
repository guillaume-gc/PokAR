using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronTailAttackAnimation : PokemonAnimationSkeleton
{
    public IronTailAttackAnimation() : base()
    {
        name = "jumping0";

        // negative predictedAnimationLength means it lasts as long as the animation
        predictedAnimationLength = -1;
        attackTarget = AttackTarget.MYSELF;

        Init();
    }
}
