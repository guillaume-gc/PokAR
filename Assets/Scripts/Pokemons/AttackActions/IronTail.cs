using System.Collections.Generic;

public class IronTail : AttackAction
{
    public IronTail()
    {
        name = "IronTail";
        contact = true;

        attackType = PokemonType.STEEL;
        attackCategory = AttackCategory.PHYSICAL;
        attackStatus = AttackStatus.NONE;
        teamStatus = TeamStatus.NONE;
        attackTarget = AttackTarget.ADVERSARY;

        defaultStats = new Dictionary<AttackStat, int>();
        defaultStats[AttackStat.POWER] = 100;
        defaultStats[AttackStat.ACCURACY] = 75;
        defaultStats[AttackStat.PP] = 15;
        defaultStats[AttackStat.PP_MAX] = 24;
        defaultStats[AttackStat.PRIORITY] = 0;
        defaultStats[AttackStat.STATUS_CHANCE] = 0;

        adversaryStatModifiers = new Dictionary<PokemonStat, StatModifier>();
        selfStatModifiers = new Dictionary<PokemonStat, StatModifier>();

        pokemonAnimations = new PokemonAnimation[]
        {
            new IronTailAttackAnimation(),
            new PhysicalAttackHurt()
        };

        adversaryStatModifiers[PokemonStat.DEFENSE] = new StatModifier(-1, 30);

        Init();
    }
}
