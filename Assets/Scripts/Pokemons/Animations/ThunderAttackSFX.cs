using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class ThunderAttackSFX : PokemonAnimationSFX
{
    public ThunderAttackSFX() : base()
    {
        name = "lightningBolt";

        predictedAnimationLength = 1.0f;

        sfxPrefab = SFXPrefab.SIMPLE_LIGHTNING_BOLT;
        attackTarget = AttackTarget.MYSELF;

        Init();
    }

    protected override int ManageStartScript()
    {
        int i = 0;

        SFXPrefabInstances = new List<GameObject>();

        foreach (PokemonBehaviour destination in destinationsPokemonBehaviour)
        {
            GameObject clone = GameObject.Instantiate(sfxPrefabObject);
            logs.Debug($"Created prefab clone ({clone.GetInstanceID()}) from {sfxPrefabObject.GetInstanceID()}");

            SFXPrefabInstances.Add(clone);

            LightningBoltScript lightningBoltScript = clone.GetComponent<LightningBoltScript>();

            lightningBoltScript.StartObject = originPokemonBehaviour.gameObject;
            lightningBoltScript.EndObject = destination.gameObject;

            i++;
        }

        return i;
    }
}
