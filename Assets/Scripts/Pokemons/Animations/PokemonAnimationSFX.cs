using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimationSFX : PokemonAnimation
{
    protected IList<GameObject> SFXPrefabInstances;

    protected PokemonAnimationSFX() : base()
    {
        animationType = AnimationType.SFX;
    }

    override public void StartAnimation()
    {
        int i = ManageStartScript();

        logs.Info($"Created {i} prefab instances");

    }

    protected virtual int ManageStartScript()
    {
        return 0;
    }

    override public void StopAnimation()
    {
        int i;

        for (i = 0; i < SFXPrefabInstances.Count; i++)
        {
            logs.Debug($"Destroying {SFXPrefabInstances[i].GetInstanceID()}");
            GameObject.DestroyImmediate(SFXPrefabInstances[i]);
        }

        logs.Info($"Destroyed {i} prefab instances");
    }
}
