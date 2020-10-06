using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackAction : MonoBehaviour
{
    protected string name;
    protected bool contact;

    protected PokemonType attackType;
    protected AttackCategory attackCategory;
    protected AttackStatus attackStatus;
    protected TeamStatus teamStatus;
    protected AttackTarget attackTarget;

    protected IDictionary<AttackStat, int> defaultStats;
    protected IDictionary<AttackStat, int> effectiveStats;

    protected IDictionary<PokemonStat, StatModifier> adversaryStatModifiers;
    protected IDictionary<PokemonStat, StatModifier> selfStatModifiers;

    protected PokemonAnimation[] pokemonAnimations;

    public string Name { get => name; }
    public bool Contact { get => contact; }

    public PokemonType AttackType { get => attackType; }
    public AttackCategory AttackCategory { get => attackCategory; }
    public AttackStatus AttackStatus { get => attackStatus; }
    public TeamStatus TeamStatus { get => teamStatus; }

    public IDictionary<AttackStat, int> DefaultStats { get => defaultStats; set => defaultStats = value; }
    public IDictionary<AttackStat, int> EffectiveStats { get => effectiveStats; set => effectiveStats = value; }

    public IDictionary<PokemonStat, StatModifier> AdversaryStatModifiers { get => adversaryStatModifiers; }
    public IDictionary<PokemonStat, StatModifier> SelfStatModifiers { get => selfStatModifiers; }

    public PokemonAnimation[] PokemonAnimations { get => pokemonAnimations; }
    public AttackTarget AttackTarget { get => attackTarget; }

    protected AttackAction()
    {
        // Sample on how to create an attack action, do not used or call
        name = "none";
        contact = false;

        attackType = PokemonType.NONE;
        attackCategory = AttackCategory.PHYSICAL;
        attackStatus = AttackStatus.NONE;
        teamStatus = TeamStatus.NONE;
        attackTarget = AttackTarget.NONE;

        defaultStats = new Dictionary<AttackStat, int>();
        defaultStats[AttackStat.POWER] = 0;
        defaultStats[AttackStat.ACCURACY] = 0;
        defaultStats[AttackStat.PP] = 0;
        defaultStats[AttackStat.PP_MAX] = 0;
        defaultStats[AttackStat.PRIORITY] = 0;
        defaultStats[AttackStat.STATUS_CHANCE] = 0;

        adversaryStatModifiers = new Dictionary<PokemonStat, StatModifier>();
        selfStatModifiers = new Dictionary<PokemonStat, StatModifier>();

        pokemonAnimations = new PokemonAnimation[0];

        Init();
    }

    protected void Init()
    {
        effectiveStats = new Dictionary<AttackStat, int>(defaultStats);
    }
}
