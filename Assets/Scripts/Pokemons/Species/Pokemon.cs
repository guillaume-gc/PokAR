using System.Collections;
using System.Collections.Generic;
using System;

public class Pokemon
{
    protected static int GLOBAL_IDENTIFIER_STATE = -1;
    protected static int MAX_MODIFIER = 6;
    protected static int MIN_MODIFIER = -6;
    protected static List<PokemonStat> MODIFIER_ALLOWED = new List<PokemonStat>(new PokemonStat[] {
        PokemonStat.ATTACK,
        PokemonStat.DEFENSE,
        PokemonStat.SP_ATTACK,
        PokemonStat.SP_DEFENSE,
        PokemonStat.SPEED 
    });
    protected static Random RNG = new Random();

    // This dictionary allows to automatically select the right stat for the right attack category, example attackCategoryStats[AttackCategory.PHYSICAL]["defense"] will return PokemonStat.DEFENSE
    protected IDictionary<AttackCategory, IDictionary<string, PokemonStat>> attackCategoryStats = new Dictionary<AttackCategory, IDictionary<string, PokemonStat>>
    {
        { AttackCategory.PHYSICAL,  new Dictionary<string, PokemonStat> {{ "attack", PokemonStat.ATTACK }, { "defense", PokemonStat.DEFENSE } } },
        { AttackCategory.SPECIAL,  new Dictionary<string, PokemonStat> {{ "attack", PokemonStat.SP_ATTACK }, { "defense", PokemonStat.SP_DEFENSE } } }
    };

    protected string name;

    // Pokemon level, is currently always 100
    protected int level;

    // Pokemon national identifier, used to identify pokemon species
    protected int nationalIdentifier;

    // Pokemon unique identifier, everypokemon has a different unique identifier
    protected int uniqueIdentifier;

    protected IDictionary<PokemonStat, int> defaultStats;
    protected IDictionary<PokemonStat, int> effectiveStats;

    protected PokemonType type;

    // Pokemon attacks (not to mix up with attack stats)
    protected AttackAction[] attackActions;

    protected Logs logs;

    public string Name { get => name; }
    public int Level { get => level; set => level = value; }
    public int NationalIdentifier { get => nationalIdentifier; }
    public int UniqueIdentifier { get => uniqueIdentifier; }

    public PokemonType Type { get => type; }
    public AttackAction[] AttackActions { get => attackActions; }
    public IDictionary<PokemonStat, int> DefaultStats { get => defaultStats; }
    public IDictionary<PokemonStat, int> EffectiveStats { get => effectiveStats; }

    public Pokemon()
    {
        name = "GenericPokemon";
        level = 100;
        nationalIdentifier = -1;

        defaultStats = new Dictionary<PokemonStat, int>();

        defaultStats[PokemonStat.HP] = 0;
        defaultStats[PokemonStat.HP_MAX] = 0;
        defaultStats[PokemonStat.ATTACK] = 0;
        defaultStats[PokemonStat.DEFENSE] = 0;
        defaultStats[PokemonStat.SP_ATTACK] = 0;
        defaultStats[PokemonStat.SP_DEFENSE] = 0;
        defaultStats[PokemonStat.SPEED] = 0;

        attackActions = new AttackAction[0];

        type = PokemonType.NONE;

        Init();
    }

    protected void Init()
    {
        effectiveStats = new Dictionary<PokemonStat, int>(defaultStats);

        uniqueIdentifier = ++GLOBAL_IDENTIFIER_STATE;

        logs = new Logs(name + ":" + uniqueIdentifier);
    }

    public string GetTypeName()
    {
        return Enum.GetName(typeof(PokemonType), (int)type);
    }

    public bool IsKO()
    {
        return effectiveStats[PokemonStat.HP] <= 0;
    }

    public void Reset()
    {
        effectiveStats = new Dictionary<PokemonStat, int>(defaultStats);
    }

    public void ReduceHP(int damage)
    {
        effectiveStats[PokemonStat.HP] -= damage;
        if (effectiveStats[PokemonStat.HP] < 0)
        {
            effectiveStats[PokemonStat.HP] = 0;
        }

        logs.Info($"Has now {effectiveStats[PokemonStat.HP]} HP.");
    }

    public void Heal(int heal)
    {
        effectiveStats[PokemonStat.HP] += heal;
        if (effectiveStats[PokemonStat.HP] > effectiveStats[PokemonStat.HP_MAX])
        {
            effectiveStats[PokemonStat.HP] = effectiveStats[PokemonStat.HP_MAX];
        }

        logs.Info($"Has now {effectiveStats[PokemonStat.HP]} HP.");
    } 

    public void applyModifierToStat(int modifier, PokemonStat pokemonStat)
    {

    }

    public void AttackPokemon(Pokemon target, int attackIndex)
    {
        if (attackIndex < 0 || attackIndex > attackActions.Length)
        {
            logs.Warn($"Attack Index {attackIndex} is out of bound (attacks length is {attackActions.Length}).");
        }

        AttackAction attackAction = attackActions[attackIndex];

        logs.Info($"Attack {target.Name}");

        target.IsAttackedBy(this, attackAction);
    }

    public void AttackPokemon(Pokemon target, AttackAction attackAction)
    {
        target.IsAttackedBy(this, attackAction);
    }

    public void IsAttackedBy(Pokemon attacker, AttackAction attackAction)
    {
        logs.Info($"Is Attacked by {attacker.Name}. Trying to evade...");

        int rngValue = RNG.Next(1, 101);
        int precision = attackAction.EffectiveStats[AttackStat.ACCURACY];

        logs.Info($"Generated value is {rngValue}, attack precision is {precision}.");

        if (rngValue > precision)
        {
            logs.Info("Succesfully evaded attack.");
            return;
        }
        else
        {
            logs.Info("Evasion failed.");
        }

        if (attackAction.AttackCategory == AttackCategory.STATUS)
        {
            logs.Info("Attack category is status, no damage.");
        }
        else
        {
            logs.Info("Attack category is not status, will be damaged.");
            IsTargetedByAttack(attacker, attackAction);
        }
    }

    private void IsTargetedByAttack(Pokemon attacker, AttackAction attackAction)
    {
        int damage;

        PokemonStat attackEffectiveStat;
        PokemonStat defenseEffectiveStat;

        int attackEffectiveStatValue;
        int defenseEffectiveStatValue;

        int attackPower = attackAction.EffectiveStats[AttackStat.POWER];

        double modifier;

        // Get the right Pokemon stats (attack & defense) for the current attack
        attackEffectiveStat = attackCategoryStats[attackAction.AttackCategory]["attack"];
        defenseEffectiveStat = attackCategoryStats[attackAction.AttackCategory]["defense"];

        // Get the right Pokemon stat values for the current attack
        attackEffectiveStatValue = effectiveStats[attackEffectiveStat];
        defenseEffectiveStatValue = effectiveStats[defenseEffectiveStat];

        modifier = 1;

        damage = (int)Math.Ceiling(((((((2 * attacker.Level) / 5) + 2) * attackPower * (attackEffectiveStatValue / defenseEffectiveStatValue)) / 50) + 2) * modifier);

        logs.Info($"Lost {damage} HP.");

        ReduceHP(damage);
    }
}
