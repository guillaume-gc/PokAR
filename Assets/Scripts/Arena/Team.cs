using System;
using System.Collections;
using System.Collections.Generic;

public class Team
{
    private static int MAX_POKEMON = 1;
    private static int LAST_TEAM_IDENTIFIER = -1;
    private static string PREFIX = "team";

    private List<Pokemon> pokemons;
    private int identifier;
    private string name;

    private TeamStatus teamStatus;

    // The action the team will perform during this turn
    private AttackAction preparedAttackAction;

    private Logs logs;

    private IDictionary<TeamStatus, Action> teamStatusDelegates;

    // Behaviour of the pokemon currently fighting in the Arena
    private PokemonBehaviour currentPokemonBehaviour;

    public List<Pokemon> Pokemons { get => pokemons; }
    public int Identifier { get => identifier; }
    public string Name { get => name; }
    public PokemonBehaviour CurrentPokemonBehaviour { get => currentPokemonBehaviour; set => currentPokemonBehaviour = value; }
    public AttackAction PreparedAttackAction { get => preparedAttackAction; set => preparedAttackAction = value; }
    public TeamStatus TeamStatus { get => teamStatus; set => teamStatus = value; }

    public Team(int identifier = -1)
    {
        pokemons = new List<Pokemon>();

        LAST_TEAM_IDENTIFIER++;

        if (identifier < 0)
        {
            this.identifier = LAST_TEAM_IDENTIFIER;
        }
        else
        {
            this.identifier = identifier;
        }

        name = PREFIX + identifier;

        logs = new Logs(name);

        preparedAttackAction = null;

        teamStatus = TeamStatus.NONE;

        teamStatusDelegates = new Dictionary<TeamStatus, Action>();
        teamStatusDelegates[TeamStatus.NONE] = () => {
            logs.Info($"Team {name} has no team status");
        };
        teamStatusDelegates[TeamStatus.HEAL_USER_HALF] = () => this.HealHalfUserStatus();
    }

    public void Reset()
    {
        preparedAttackAction = null;

        teamStatus = TeamStatus.NONE;

        foreach (Pokemon pokemon in pokemons)
        {
            currentPokemonBehaviour.IdlingAllowed = true;

            pokemon.Reset();
        }
    }

    public void ResolveTeamStatus()
    {
        try
        {
            teamStatusDelegates[teamStatus].Invoke();
        }
        catch (Exception e)
        {
            logs.Warn($"Could not resolve team status, reason {e.GetType().ToString()} -> {e.ToString()}");
        }
    }

    public void HealHalfUserStatus()
    {
        logs.Info($"Resolving heal half user status");

        Pokemon pokemon = currentPokemonBehaviour.Pokemon;

        int heal = pokemon.EffectiveStats[PokemonStat.HP] / 2;

        pokemon.Heal(heal);
    }

    public void addPokemon(Pokemon pokemonToAdd)
    {
        if (pokemons.Count >= MAX_POKEMON)
        {
            throw new TooManyPokemonsException();
        }

        pokemons.Add(pokemonToAdd);

        teamStatus = TeamStatus.NONE;
    }

    public void AddUniquePokemon(Pokemon pokemonToAdd)
    {
        if (pokemons.Count >= MAX_POKEMON)
        {
            throw new TooManyPokemonsException();
        }

        bool pokemonDoesNotExist = true;
        foreach (Pokemon pokemon in pokemons)
        {
            if (pokemon.UniqueIdentifier == pokemonToAdd.UniqueIdentifier)
            {
                pokemonDoesNotExist = false;
                break;
            }
        }

        if (pokemonDoesNotExist)
        {
            logs.Info($"Add Pokemon {pokemonToAdd.Name}");
            pokemons.Add(pokemonToAdd);
        }
    }

    public bool IsFull()
    {
        return (pokemons.Count == MAX_POKEMON);
    }
}
