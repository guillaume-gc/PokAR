using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonAnimation
{
    static protected int LAST_IDENTIFIER = -1;
    protected int identifier;

    protected float globalTimer;

    // Is predicted because it's not possible to know at the exact time the animation starts
    protected float predictedAnimationStartTime;
    // Same logic
    protected float predictedAnimationLength;
    // When was the animation started?
    protected float animationStartedTime;

    // Some animation will use an Unity Prefab, saving prefab type and prefab object
    protected SFXPrefab sfxPrefab;
    protected GameObject sfxPrefabObject;

    protected AnimationType animationType;

    protected bool animating;
    protected bool completed;

    // An animation may affect the acting pokemon or another pokemon 
    protected AttackTarget attackTarget;

    protected PokemonBehaviour originPokemonBehaviour;
    protected IList<PokemonBehaviour> destinationsPokemonBehaviour;

    protected string name;

    protected Logs logs;

    public int Identifier { get => identifier; set => identifier = value; }
    public float GlobalTimer { get => globalTimer; }
    public float PredictedAnimationStartTime { get => predictedAnimationStartTime; set => predictedAnimationStartTime = value; }
    public float PredictedAnimationLength { get => predictedAnimationLength; }
    public float AnimationStartedTime { get => animationStartedTime; }
    public SFXPrefab SFXPrefab { get => sfxPrefab; set => sfxPrefab = value; }
    public GameObject SFXPrefabObject { get => sfxPrefabObject; set => sfxPrefabObject = value; }
    public bool Animating { get => animating; }
    public bool Completed { get => completed; set => completed = value; }
    public AttackTarget AttackTarget { get => attackTarget; set => attackTarget = value; }
    public PokemonBehaviour OriginPokemonBehaviour { get => originPokemonBehaviour; set => originPokemonBehaviour = value; }
    public IList<PokemonBehaviour> DestinationsPokemonBehaviour { get => destinationsPokemonBehaviour; set => destinationsPokemonBehaviour = value; }
    public string Name { get => name; set => name = value; }

    protected PokemonAnimation()
    {
        LAST_IDENTIFIER++;
        identifier = LAST_IDENTIFIER;
    }

    protected void Init()
    {
        logs = new Logs(name + ":" + Identifier);
    }

    public void InitAnimation(PokemonBehaviour originPokemonBehaviour, IList<PokemonBehaviour> destinationsPokemonBehaviour, float predictedAnimationStartTime)
    {
        globalTimer = 0;
        this.predictedAnimationStartTime = predictedAnimationStartTime;

        animating = false;
        completed = false;

        this.originPokemonBehaviour = originPokemonBehaviour;
        this.destinationsPokemonBehaviour = destinationsPokemonBehaviour;

        if (predictedAnimationLength < 0)
        {
            GetPredictedAnimationLengthFromPokemonBehaviour(originPokemonBehaviour);
        }

        animationStartedTime = 0;
    }

    virtual public void StartAnimation()
    {

    }

    virtual public void StopAnimation()
    {

    }

    public void Update()
    {
        if (completed)
        {
            logs.Debug("Animation completed, no need to update.");
            return;
        }

        try
        {
            globalTimer += UnityEngine.Time.deltaTime;

            if (globalTimer >= predictedAnimationStartTime && !animating)
            {
                logs.Info($"Start animation at {globalTimer}.");

                animating = true;

                animationStartedTime = globalTimer;

                StartAnimation();
            }
            else if (globalTimer >= animationStartedTime + predictedAnimationLength && animating)
            {
                logs.Info($"Stop animation at {globalTimer}.");

                animating = false;
                completed = true;

                StopAnimation();
            }
        }
        catch (PokemonAnimationException pae)
        {
            logs.Warn($"Could not update pokemon, reason: {pae.ToString()}");
        }
    }

    protected void GetPredictedAnimationLengthFromPokemonBehaviour(PokemonBehaviour pokemonBehaviour)
    {
        if (!pokemonBehaviour.AnimationTime.ContainsKey(name))
        {
            throw new PokemonAnimationException($"pokemon {pokemonBehaviour.name} has no animation named {name}");
        }

        predictedAnimationLength = pokemonBehaviour.AnimationTime[name];
    }
}
