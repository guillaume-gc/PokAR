using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBehaviour : MonoBehaviour
{
    public GameObject simpleLightningBoltAnimatedPrefab;

    private Arena arena;
    private Logs logs;
    private IDictionary<ArenaStage, Action> stageDelegates;
    private Team[] battleOrder;

    private IDictionary<SFXPrefab, GameObject> sfxPrefabs;

    private bool animationStarted;

    public Arena Arena { get => arena;}
    public Team[] BattleOrder { get => battleOrder; }

    void Start()
    {
        arena = new Arena();
        logs = new Logs("ArenaBehaviour");

        StageDelegatesInit();
        SFXPrefabsInit();
    }

    void SFXPrefabsInit()
    {
        sfxPrefabs = new Dictionary<SFXPrefab, GameObject>();

        sfxPrefabs[SFXPrefab.SIMPLE_LIGHTNING_BOLT] = simpleLightningBoltAnimatedPrefab;
        sfxPrefabs[SFXPrefab.NONE] = null;
    }

    void StageDelegatesInit()
    {
        // Create a list of method, each method being a different stage during the arena
        stageDelegates = new Dictionary<ArenaStage, Action>();

        // Insert "Wait for pokemons" stage
        stageDelegates[ArenaStage.WAITING_FOR_POKEMONS] = () => this.WaitingForPokemonsStage();

        // Insert team 0 & 1 decision stage
        stageDelegates[ArenaStage.TEAM0_DECISION] = () => this.Team0DecisionStage();
        stageDelegates[ArenaStage.TEAM1_DECISION] = () => this.Team1DecisionStage();

        // Prepare for combat
        stageDelegates[ArenaStage.COMBAT_INIT] = () => this.CombatInitStage();

        // Insert team 0 & 1 team status resolve stage
        stageDelegates[ArenaStage.TEAM0_STATUS_RESOLVE] = () => this.Team0StatusResolveStage();
        stageDelegates[ArenaStage.TEAM1_STATUS_RESOLVE] = () => this.Team1StatusResolveStage();

        // Insert first combat stages
        stageDelegates[ArenaStage.FIRST_COMBAT_COMPUTE] = () => this.FirstCombatComputeStage();
        stageDelegates[ArenaStage.FIRST_COMBAT_ANIMATION] = () => this.FirstCombatAnimationStage();

        // Insert second combat stages
        stageDelegates[ArenaStage.SECOND_COMBAT_COMPUTE] = () => this.SecondCombatComputeStage();
        stageDelegates[ArenaStage.SECOND_COMBAT_ANIMATION] = () => this.SecondCombatAnimationStage();

        // Insert conclusion stage
        stageDelegates[ArenaStage.CONCLUSION] = () => this.ConclusionStage();
    }

    void Update()
    {
        // Dynamically call current arena stage routine
        if (!stageDelegates.ContainsKey(arena.ArenaStage))
        {
            logs.Warn($"{Enum.GetName(typeof(ArenaStage), arena.ArenaStage)} key does not exist");
            return;
        }
        stageDelegates[arena.ArenaStage].Invoke();

        // When all pokemons are here, they will MENACINGLY stare into each other's eyes
        if (arena.ArenaStage != ArenaStage.WAITING_FOR_POKEMONS)
        {
            MenacingStare();
        }
    }

    private void MenacingStare()
    {
        PokemonBehaviour team0CurrentPokemonBehaviour = arena.Team0.CurrentPokemonBehaviour;
        PokemonBehaviour team1CurrentPokemonBehaviour = arena.Team1.CurrentPokemonBehaviour;

        team0CurrentPokemonBehaviour.MenacingLookAt(team1CurrentPokemonBehaviour.gameObject);
        team1CurrentPokemonBehaviour.MenacingLookAt(team0CurrentPokemonBehaviour.gameObject);
    }

    public void WaitingForPokemonsStage()
    {
        logs.Debug("Waiting for Pokemon Stage.");

        if (!arena.Team0.IsFull())
        {
            FillTeam(arena.Team0);
        }
        else if (!arena.Team1.IsFull())
        {
            FillTeam(arena.Team1);
        }
        else
        {
            logs.Info("Move to Team 0 Decision Stage.");
            arena.ArenaStage = ArenaStage.TEAM0_DECISION;
        }
    }

    public void Team0DecisionStage()
    {
        bool actionPlayed = TeamDecision(arena.Team0);

        if (actionPlayed)
        {
            logs.Info("Move to Team 1 Decision Stage.");
            arena.ArenaStage = ArenaStage.TEAM1_DECISION;
        }
    }

    public void Team1DecisionStage()
    {
        bool actionPlayed = TeamDecision(arena.Team1);

        if (actionPlayed)
        {
            logs.Info("Move to Combat Stage.");
            arena.ArenaStage = ArenaStage.COMBAT_INIT;
        }
    }

    public void CombatInitStage()
    {
        logs.Debug("Combat Initialisation Stage.");

        battleOrder = arena.FindBattleOrder();

        logs.Info("Move to first round combat.");
        arena.ArenaStage = ArenaStage.TEAM0_STATUS_RESOLVE;
    }

    public void Team0StatusResolveStage()
    {
        logs.Debug("Team 0 status resolution Stage.");

        arena.Team0.ResolveTeamStatus();

        logs.Info("Move to team 1 status resolution Stage.");

        arena.ArenaStage = ArenaStage.TEAM1_STATUS_RESOLVE;
    }

    public void Team1StatusResolveStage()
    {
        logs.Debug("Team 1 status resolution Stage.");

        arena.Team1.ResolveTeamStatus();

        logs.Info("Move to first combat compute Stage.");

        arena.ArenaStage = ArenaStage.FIRST_COMBAT_COMPUTE;
    }

    public void FirstCombatComputeStage()
    {
        logs.Debug("Combat First Round combat Stage.");

        if (battleOrder[0].CurrentPokemonBehaviour.Pokemon.IsKO())
        {
            logs.Info($"Pokemon is KO. Team {battleOrder[0].Name} will not attack.");
            logs.Info("Move to Conclusion Stage.");

            arena.ArenaStage = ArenaStage.CONCLUSION;

            return;
        }

        CombatPhase(battleOrder[0], battleOrder[1]);

        logs.Info("Move to First Round Animation.");
        animationStarted = false;
        arena.ArenaStage = ArenaStage.FIRST_COMBAT_ANIMATION;
    }

    private void FirstCombatAnimationStage()
    {
        logs.Debug("First Combat Animation Stage.");

        if (Animation(battleOrder[0], battleOrder[1]))
        {
            logs.Info("Move to Second Round combat.");
            arena.ArenaStage = ArenaStage.SECOND_COMBAT_COMPUTE;
        }
    }

    public void SecondCombatComputeStage()
    {
        logs.Debug("Second Combat Compute Stage.");

        if (battleOrder[1].CurrentPokemonBehaviour.Pokemon.IsKO())
        {
            logs.Info($"Pokemon is KO. Team {battleOrder[1].Name} will not attack.");
            logs.Info("Move to conclusion.");

            arena.ArenaStage = ArenaStage.CONCLUSION;

            return;
        }

        CombatPhase(battleOrder[1], battleOrder[0]);

        logs.Info("Move to Second Round Animation.");
        animationStarted = false;
        arena.ArenaStage = ArenaStage.SECOND_COMBAT_ANIMATION;
    }

    private void SecondCombatAnimationStage()
    {
        logs.Debug("Combat Second Round animation stage.");

        if (Animation(battleOrder[1], battleOrder[0]))
        {
            logs.Info("Turn finished. Move to team 0 turn.");
            arena.ArenaStage = ArenaStage.TEAM0_DECISION;
        }
    }

    public void ConclusionStage()
    {
        logs.Debug("Conclusion stage.");

        bool actionPlayed = KeyBoardConclusionStage();

        if (actionPlayed)
        {
            arena.ArenaStage = ArenaStage.TEAM0_DECISION;
        }
    }

    private bool TeamDecision(Team currentTeam)
    {
        return KeyBoardTeamDecisionInput(currentTeam);
    }

    private void CombatPhase(Team currentTeam, Team adversaryTeam)
    {
        logs.Info($"Team {currentTeam.Name} play.");

        AttackAction attackAction = currentTeam.PreparedAttackAction;

        currentTeam.TeamStatus = attackAction.TeamStatus;

        logs.Info($"{currentTeam.Name} team status is now {currentTeam.TeamStatus}.");

        Pokemon currentTeamPokemon = currentTeam.CurrentPokemonBehaviour.Pokemon;
        Pokemon adversaryTeamPokemon = adversaryTeam.CurrentPokemonBehaviour.Pokemon;

        // The team attack the other team with their prepared attack
        if (attackAction.AttackTarget == AttackTarget.ADVERSARY)
        {
            currentTeamPokemon.AttackPokemon(adversaryTeamPokemon, attackAction);
        }
        else
        {
            logs.Info($"Attack {attackAction.Name} targets user");
        }
        
    }

    public bool Animation(Team currentTeam, Team adversaryTeam)
    {
        bool animationCompleted = false;

        // Get current animation from attack
        AttackAction preparedAttackAction = currentTeam.PreparedAttackAction;

        if (!animationStarted)
        {
            animationStarted = true;

            for (int i = 0; i < preparedAttackAction.PokemonAnimations.Length; i++)
            {
                PokemonAnimation pokemonAnimation = preparedAttackAction.PokemonAnimations[i];

                InitAnimation(pokemonAnimation, currentTeam, adversaryTeam, i);
            }
        }
        else
        {
            foreach (PokemonAnimation pokemonAnimation in preparedAttackAction.PokemonAnimations)
            {
                pokemonAnimation.Update();
            }

            int countAnimationsCompleted = 0;

            foreach (PokemonAnimation pokemonAnimation in preparedAttackAction.PokemonAnimations)
            {
                if (pokemonAnimation.Completed)
                {
                    countAnimationsCompleted++;
                }
            }

            animationCompleted = (countAnimationsCompleted == preparedAttackAction.PokemonAnimations.Length);

            if (animationCompleted)
            {
                currentTeam.CurrentPokemonBehaviour.IdlingAllowed = true;
                adversaryTeam.CurrentPokemonBehaviour.IdlingAllowed = true;
            }
        }

        return animationCompleted;
    }

    private void InitAnimation(PokemonAnimation pokemonAnimation, Team currentTeam, Team adversaryTeam, int animationIndex)
    {
        currentTeam.CurrentPokemonBehaviour.IdlingAllowed = false;
        adversaryTeam.CurrentPokemonBehaviour.IdlingAllowed = false;

        logs.Debug($"Initializing animation {pokemonAnimation.Name}");

        PokemonBehaviour originPokemonBehaviour;
        IList<PokemonBehaviour> destinationPokemonsBehaviour = new List<PokemonBehaviour>();

        // If necessary, give the sfx prefab object (for sfx animations)
        pokemonAnimation.SFXPrefabObject = sfxPrefabs[pokemonAnimation.SFXPrefab];

        // Check if animation target playing Pokemon or its opponant
        if (pokemonAnimation.AttackTarget == AttackTarget.MYSELF)
        {
            originPokemonBehaviour = currentTeam.CurrentPokemonBehaviour;
            destinationPokemonsBehaviour.Add(adversaryTeam.CurrentPokemonBehaviour);
        }
        else
        {
            logs.Debug($"Playing animation {pokemonAnimation.Name}");

            originPokemonBehaviour = adversaryTeam.CurrentPokemonBehaviour;
            destinationPokemonsBehaviour.Add(currentTeam.CurrentPokemonBehaviour);
        }

        // Find out when the animation is supposed to start (it depends on the attack and the pokemon)
        Dictionary<string, float[]> predictedAnimationStartTimes = (Dictionary<string, float[]>)originPokemonBehaviour.PredictedAnimationStartTimes;

        string attackActionName = currentTeam.PreparedAttackAction.Name;

        if (animationIndex >= predictedAnimationStartTimes[attackActionName].Length)
        {
            throw new PokemonAnimationException($"animationIndex {animationIndex} is out of bound to {originPokemonBehaviour.Pokemon.Name} " +
                $"attack {attackActionName} predictedAnimationStartTime (length {predictedAnimationStartTimes[attackActionName].Length})");
        }

        float predictedAnimationStartTime = predictedAnimationStartTimes[attackActionName][animationIndex];

        pokemonAnimation.InitAnimation(originPokemonBehaviour, destinationPokemonsBehaviour, predictedAnimationStartTime);
    }

    private bool KeyBoardTeamDecisionInput(Team currentTeam)
    {
        bool actionPlayed = false;

        try
        {
            if (Input.GetKeyUp("a"))
            {
                AttackAction attackAction = currentTeam.CurrentPokemonBehaviour.Pokemon.AttackActions[0];

                logs.Debug($"'a' key pressed, team {currentTeam.Name} will use attack {attackAction.Name}");

                currentTeam.PreparedAttackAction = attackAction;

                actionPlayed = true;
            }
            else if (Input.GetKeyUp("z"))
            {
                AttackAction attackAction = currentTeam.CurrentPokemonBehaviour.Pokemon.AttackActions[1];

                logs.Debug($"'z' key pressed, team {currentTeam.Name} will use attack {attackAction.Name}");

                currentTeam.PreparedAttackAction = attackAction;

                actionPlayed = true;
            }
            else if (Input.GetKeyUp("e"))
            {
                AttackAction attackAction = currentTeam.CurrentPokemonBehaviour.Pokemon.AttackActions[2];

                logs.Debug($"'e' key pressed, team {currentTeam.Name} will use attack {attackAction.Name}");

                currentTeam.PreparedAttackAction = attackAction;

                actionPlayed = true;
            }
            else if (Input.GetKeyUp("r"))
            {
                AttackAction attackAction = currentTeam.CurrentPokemonBehaviour.Pokemon.AttackActions[3];

                logs.Debug($"'r' key pressed, team {currentTeam.Name} will use attack {attackAction.Name}");

                currentTeam.PreparedAttackAction = attackAction;

                actionPlayed = true;
            }
        }
        catch (Exception e) {
            logs.Warn($"Could not choose attack, reason: {e.GetType().ToString()} -> {e.ToString()}");
            actionPlayed = false;
        }

        return actionPlayed;
    }

    private bool KeyBoardConclusionStage()
    {
        bool actionPlayed = false;
        int hp = arena.Team0.CurrentPokemonBehaviour.Pokemon.EffectiveStats[PokemonStat.HP];
        int hpMax = arena.Team0.CurrentPokemonBehaviour.Pokemon.EffectiveStats[PokemonStat.HP_MAX];
        
        if (Input.GetKeyUp("space"))
        {
            logs.Debug($"'space' key pressed, reset teams' pokemons");
            logs.Info("fhdsjkhfdkjshfklsdfhkshfkdshkhfdk" + hp.ToString());
            arena.Team0.Reset();
            arena.Team1.Reset();

            actionPlayed = true;
        }

        return actionPlayed;
    }
    
    private void FillTeam(Team team)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(team.Name);
        PokemonBehaviour pokemonBehaviour = null;

        logs.Info($"Filling team {team.Name}");

        foreach (GameObject gameObject in gameObjects)
        {
            pokemonBehaviour = gameObject.GetComponent<PokemonBehaviour>();
            team.AddUniquePokemon(pokemonBehaviour.Pokemon);
        }

        // As of now, there is no pokemon selection, so one pokemon is selected if the team is full
        if (team.IsFull())
        {
            if (pokemonBehaviour == null)
            {
                logs.Error("No pokemon behaviour found");
                throw new NullReferenceException("no pokemon behaviour found");
            }

            logs.Info($"Team {team.Name} is now full");

            team.CurrentPokemonBehaviour = pokemonBehaviour;
        }
    }
}
