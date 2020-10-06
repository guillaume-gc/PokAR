using System.Collections.Generic;

public class Thunderbolt : AttackAction
{
    public Thunderbolt()
    {
        name = "Thunderbolt";
        contact = false;

        attackType = PokemonType.ELECTRIC;
        attackCategory = AttackCategory.SPECIAL;
        attackStatus = AttackStatus.PARALYZE;
        teamStatus = TeamStatus.NONE;
        attackTarget = AttackTarget.ADVERSARY;

        defaultStats = new Dictionary<AttackStat, int>();
        defaultStats[AttackStat.POWER] = 90;
        defaultStats[AttackStat.ACCURACY] = 100;
        defaultStats[AttackStat.PP] = 15;
        defaultStats[AttackStat.PP_MAX] = 24;
        defaultStats[AttackStat.PRIORITY] = 0;
        defaultStats[AttackStat.STATUS_CHANCE] = 10;

        adversaryStatModifiers = new Dictionary<PokemonStat, StatModifier>();
        selfStatModifiers = new Dictionary<PokemonStat, StatModifier>();

        pokemonAnimations = new PokemonAnimation[]
        {
            new ThunderAttackAnimation(),
            new ThunderAttackSFX(),
            new ThunderAttackHurt()
        };

        Init();
    }
}
