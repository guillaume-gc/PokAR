using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PokemonAnimationSkeleton : PokemonAnimation
{
    protected PokemonAnimationSkeleton() : base()
    {
        animationType = AnimationType.SKELETAL;
        sfxPrefab = SFXPrefab.NONE;
    }

    override public void StartAnimation()
    {
        originPokemonBehaviour.PlayDefinedAnimation(name);

        logs.Info($"Playing {name} animation");
    }

    override public void StopAnimation()
    {
        originPokemonBehaviour.StopDefinedAnimation(name);

        logs.Info($"Stopped {name} animation");
    }
}
