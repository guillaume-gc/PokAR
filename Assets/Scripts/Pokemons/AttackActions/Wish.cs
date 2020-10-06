using System.Collections.Generic;

public class Wish : AttackAction
{
    public Wish()
    {
        name = "Thunderbolt";
        contact = false;

        attackType = PokemonType.NORMAL;
        attackCategory = AttackCategory.SPECIAL;
        attackStatus = AttackStatus.PARALYZE;
        teamStatus = TeamStatus.HEAL_USER_HALF;
        attackTarget = AttackTarget.MYSELF;

        defaultStats = new Dictionary<AttackStat, int>();
        defaultStats[AttackStat.POWER] = 0;
        defaultStats[AttackStat.ACCURACY] = 0;
        defaultStats[AttackStat.PP] = 10;
        defaultStats[AttackStat.PP_MAX] = 16;
        defaultStats[AttackStat.PRIORITY] = 0;
        defaultStats[AttackStat.STATUS_CHANCE] = 0;

        adversaryStatModifiers = new Dictionary<PokemonStat, StatModifier>();
        selfStatModifiers = new Dictionary<PokemonStat, StatModifier>();

        pokemonAnimations = new PokemonAnimation[]
        {
            new WishAttackAnimation()
        };

        Init();
    }
}
