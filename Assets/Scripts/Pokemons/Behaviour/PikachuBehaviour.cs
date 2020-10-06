using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PikachuBehaviour : PokemonBehaviour
{
    public override void Start()
    {
        pokemon = new Pikachu();

        Init();
    }

    protected override void UpdatePokemonSpecific()
    {

    }

    protected override void LoadAnimations()
    {
        animationTime = new Dictionary<string, float>();

        animationTime["attack0"] = 2.1f;
        animationTime["attack1"] = 2.5f;
        animationTime["attack2"] = 2.6f;
        animationTime["attack3"] = 2.11f;
        animationTime["attack4"] = 2.1f;
        animationTime["exhausted0"] = 4.9f;
        animationTime["falling0"] = 0.11f;
        animationTime["falling1"] = 0.19f;
        animationTime["happy0"] = 2.01f;
        animationTime["happy1"] = 2.21f;
        animationTime["happy2"] = 2.01f;
        animationTime["hurt0"] = 0.19f;
        animationTime["hurt1"] = 0.09f;
        animationTime["hurt2"] = 2.11f;
        animationTime["hurt3"] = 2.11f;
        animationTime["hurt4"] = 2.01f;
        animationTime["idle0"] = 1.09f;
        animationTime["idle1"] = 1.07f;
        animationTime["idle2"] = 0.14f;
        animationTime["idle3"] = 2.01f;
        animationTime["idle4"] = 2.01f;
        animationTime["idle5"] = 1.05f;
        animationTime["idle5"] = 1.05f;
        animationTime["idle6"] = 2.11f;
        animationTime["idle7"] = 3.17f;
        animationTime["idle7"] = 2.11f;
        animationTime["jumping0"] = 2.06f;
        animationTime["ko0"] = 1.23f;
        animationTime["lookback0"] = 1.23f;
        animationTime["run0"] = 1.23f;
        animationTime["sleep0"] = 1.23f;
        animationTime["sleep1"] = 1.23f;
        animationTime["walk0"] = 1.23f;

        animationNames = new List<string>(animationTime.Keys);

        foreach (string animationName in animationNames)
        {
            string path = $"{pokemon.Name}/Animations/{animationName}";

            // Dynamically create all animations
            try
            {
                AnimationClip clip = Instantiate(Resources.Load<AnimationClip>(path));
                clip.name = animationName;
                clip.legacy = true;

                animationComponent.AddClip(clip, animationName);
            }
            catch (ArgumentException)
            {
                logs.Warn($"Could not instantiate clip {animationName} with path {path}");
            }
        }
    }

    override public void InitPredictedAnimationStartTimes()
    {
        predictedAnimationStartTimes = new Dictionary<string, float[]>();

        predictedAnimationStartTimes["Thunderbolt"] = new float[]
        {
            0.0f,
            1.0f,
            1.0f
        };

        predictedAnimationStartTimes["QuickAttack"] = new float[]
        {
            0.0f,
            0.0f
        };

        predictedAnimationStartTimes["IronTail"] = new float[]
        {
            0.0f,
            0.0f
        };

        predictedAnimationStartTimes["Wish"] = new float[]
        {
            0.0f
        };
    }
}
