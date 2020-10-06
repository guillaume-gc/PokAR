using System;
using System.Collections;
using System.Collections.Generic;

public class Arena
{
    private static Random RNG = new Random();

    private Team team0;
    private Team team1;
    private ArenaStage arenaStage;

    private Logs logs;

    public Team Team0 { get => team0; }
    public Team Team1 { get => team1; }
    public ArenaStage ArenaStage { get => arenaStage; set => arenaStage = value; }

    public Arena()
    {
        arenaStage = ArenaStage.WAITING_FOR_POKEMONS;

        team0 = new Team(0);
        team1 = new Team(1);

        logs = new Logs("Arena");
    }

    public Team[] FindBattleOrder()
    {
        Team[] order = new Team[2];

        // Trying to find order with attack priority
        if (FindBattleOrderPriority(order))
        {
            return order;
        }

        // If fail, trying to find order with Pokemon speed
        if (FindBattleOrderSpeed(order))
        {
            return order;
        }

        // If fail, just use random
        FindBattleOrderRandom(order);

        return order;
    }

    private bool FindBattleOrderPriority(Team[] order)
    {
        logs.Info($"Trying to find battle order using priority...");

        AttackAction team0preparedAttackAction = team0.PreparedAttackAction;
        AttackAction team1preparedAttackAction = team1.PreparedAttackAction;

        int team0priority = team0preparedAttackAction.DefaultStats[AttackStat.PRIORITY];
        int team1priority = team1preparedAttackAction.DefaultStats[AttackStat.PRIORITY];

        bool orderFound = false;

        if (team0priority > team1priority)
        {
            logs.Info($"{team0.Name}'s {team0preparedAttackAction.Name} has more priority than {team1.Name}'s {team1preparedAttackAction.Name} ({team0priority} > {team1priority}).");
            logs.Info($"Team {team0.Name} play first");
            order[0] = team0;
            order[1] = team1;

            orderFound = true;
        }

        else if (team0priority < team1priority)
        {
            logs.Info($"{team0.Name}'s {team0preparedAttackAction.Name} has less priority than {team1.Name}'s {team1preparedAttackAction.Name} ({team0priority} < {team1priority}).");
            logs.Info($"Team {team1.Name} play first");
            order[0] = team1;
            order[1] = team0;

            orderFound = true;
        }
        else
        {
            logs.Info($"{team0.Name}'s {team0preparedAttackAction.Name} and {team1.Name}'s {team1preparedAttackAction.Name} have the same priority ({team0priority} = {team1priority})...");
        }

        return orderFound;
    }

    private bool FindBattleOrderSpeed(Team[] order)
    {
        logs.Info($"Trying to find battle order using speed...");

        Pokemon team0Pokemon = team0.CurrentPokemonBehaviour.Pokemon;
        Pokemon team1Pokemon = team1.CurrentPokemonBehaviour.Pokemon;

        int team0Speed = team0Pokemon.EffectiveStats[PokemonStat.SPEED];
        int team1Speed = team1Pokemon.EffectiveStats[PokemonStat.SPEED];

        bool orderFound = false;

        if (team0Speed > team1Speed)
        {
            logs.Info($"{team0.Name}'s {team0Pokemon.Name} is faster than {team1.Name}'s {team1Pokemon.Name} ({team0Speed} > {team1Speed}).");
            logs.Info($"Team {team0.Name} play first");
            order[0] = team0;
            order[1] = team1;

            orderFound = true;
        }
        else if (team0Speed < team1Speed)
        {
            logs.Info($"{team0.Name}'s {team0Pokemon.Name} is slower than {team1.Name}'s {team1Pokemon.Name} ({team0Speed} < {team1Speed}).");
            logs.Info($"Team {team1.Name} play first");
            order[0] = team1;
            order[1] = team0;

            orderFound = true;
        }
        else
        {
            logs.Info($"{team0.Name}'s {team0Pokemon.Name} and {team1.Name}'s {team1Pokemon.Name} have the same speed ({team0Speed} = {team1Speed})...");   
        }

        return orderFound;
    }

    private void FindBattleOrderRandom(Team[] order)
    {
        logs.Info($"Finding battle order using random number generator...");

        int rngValue = RNG.Next(0, 2);

        logs.Debug($"RNG value is {rngValue}");

        if (Convert.ToBoolean(rngValue))
        {
            logs.Info($"Team {team0.Name} play first.");
            order[0] = team0;
            order[1] = team1;
        }
        else
        {
            logs.Info($"Team {team1.Name} play first.");
            order[0] = team1;
            order[1] = team0;
        }
    }
}