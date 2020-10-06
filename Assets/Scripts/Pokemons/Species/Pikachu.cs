#pragma warning disable 414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikachu : Pokemon
{
    public Pikachu()
    {
        name = "Pikachu";
        level = 100;
        nationalIdentifier = 25;

        defaultStats = new Dictionary<PokemonStat, int>();

        defaultStats[PokemonStat.HP] = 211;
        defaultStats[PokemonStat.HP_MAX] = 211;
        defaultStats[PokemonStat.ATTACK] = 146;
        defaultStats[PokemonStat.DEFENSE] = 116;
        defaultStats[PokemonStat.SP_ATTACK] = 136;
        defaultStats[PokemonStat.SP_DEFENSE] = 136;
        defaultStats[PokemonStat.SPEED] = 216;

        attackActions = new AttackAction[4];

        attackActions[0] = new Thunderbolt();
        attackActions[1] = new QuickAttack();
        attackActions[2] = new IronTail();
        attackActions[3] = new Wish();

        type = PokemonType.ELECTRIC;

        Init();
    }
}
