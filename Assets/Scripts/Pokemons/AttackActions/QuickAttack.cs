using System.Collections.Generic;

public class QuickAttack : AttackAction
{
    public QuickAttack()
    {
        name = "QuickAttack";
        contact = true;

        attackType = PokemonType.NORMAL;
        attackCategory = AttackCategory.PHYSICAL;
        attackStatus = AttackStatus.NONE;

        defaultStats = new Dictionary<AttackStat, int>();
        defaultStats[AttackStat.POWER] = 40;
        defaultStats[AttackStat.ACCURACY] = 100;
        defaultStats[AttackStat.PP] = 30;
        defaultStats[AttackStat.PP_MAX] = 48;
        defaultStats[AttackStat.PRIORITY] = 1;
        defaultStats[AttackStat.STATUS_CHANCE] = 0;

        adversaryStatModifiers = new Dictionary<PokemonStat, StatModifier>();
        selfStatModifiers = new Dictionary<PokemonStat, StatModifier>();

        pokemonAnimations = new PokemonAnimation[]
        {
            new QuickAttackAnimation(),
            new PhysicalAttackHurt()
        };

        Init();
    }
}
