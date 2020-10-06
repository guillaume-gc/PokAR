using UnityEngine;
using System;
using System.Collections.Generic;

public class PokemonBehaviour : MonoBehaviour
{
    private static System.Random RNG = new System.Random();

    public int teamID;

    protected IDictionary<string, float> animationTime;
    protected IList<string> animationNames;
    protected IDictionary<string, float[]> predictedAnimationStartTimes;

    protected Pokemon pokemon;

    protected string[] logLevels;
    protected string logName;

    protected Logs logs;

    protected Animation animationComponent;

    protected string currentIdleAnimationName;

    protected bool idlingAllowed;

    public Pokemon Pokemon { get => pokemon; }
    public bool IdlingAllowed { get => idlingAllowed; set => idlingAllowed = value; }
    public IDictionary<string, float> AnimationTime { get => animationTime; }
    public IList<string> AnimationNames { get => animationNames; }
    public IDictionary<string, float[]> PredictedAnimationStartTimes { get => predictedAnimationStartTimes; set => predictedAnimationStartTimes = value; }

    public virtual void Start()
    {
        pokemon = new Pokemon();

        Init();
    }

    protected void Init()
    {
        logs = new Logs(pokemon.Name + teamID);

        animationComponent = GetComponent<Animation>();
        if (animationComponent == null)
        {
            throw new Exception("No Animation component found");
        }

        logs.Debug($"Team ID is {teamID}");

        idlingAllowed = true;

        LoadAnimations();
        InitPredictedAnimationStartTimes();
    }

    public void Update()
    {
        if (pokemon.IsKO() && idlingAllowed)
        {
            TriggerKOAnimation();
            idlingAllowed = false;
            return;
        }

        // idling is not allowed during combat
        if (idlingAllowed)
        {
            Idling();
        }

        UpdatePokemonSpecific();
    }

    private void Idling()
    {
        int playingAnimations = CountPlayingAnimations();

        // if no animation is playing, play a random idle animation
        if (playingAnimations == 0)
        {
            int idleID = RNG.Next(0, 8);
            currentIdleAnimationName = "idle" + idleID;

            animationComponent.Play(currentIdleAnimationName);
        }
    }

    private void TriggerKOAnimation()
    {
        animationComponent.Play("ko0");
    }

    protected virtual void UpdatePokemonSpecific()
    {
        
    }

    protected virtual void LoadAnimations()
    {
        animationTime = new Dictionary<string, float>();

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

    public int CountPlayingAnimations()
    {
        int count = 0;

        foreach (string animationName in animationNames)
        {
            if (animationComponent.IsPlaying(animationName))
            {
                count++;
            }
        }

        return count;
    }

    public void MenacingLookAt(GameObject target)
    {
        transform.LookAt(target.transform);
    }

    public void PlayDefinedAnimation(string animationName)
    {
        logs.Debug($"Animation name is {animationName}");
        animationComponent.Play(animationName, PlayMode.StopAll);
    }

    public void StopDefinedAnimation(string animationName)
    {
        logs.Debug($"Animation name is {animationName}");
        animationComponent.Stop(animationName);
    }

    public void StopAllAnimations()
    {
        animationComponent.Stop();
    }

    virtual public void InitPredictedAnimationStartTimes()
    {
        predictedAnimationStartTimes = new Dictionary<string, float[]>();
    }
}
