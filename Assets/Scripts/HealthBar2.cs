using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar2 : MonoBehaviour
{

    public Image Bar;
    public GameObject arenaObject;
    private ArenaBehaviour arenaBehaviour;
    public Text healthText;
    public Text AttackName;
    public Text KOText;

    // Start is called before the first frame update
    void Start()
    {
        arenaBehaviour = arenaObject.GetComponent<ArenaBehaviour>();
        Bar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (arenaBehaviour.Arena.ArenaStage != ArenaStage.WAITING_FOR_POKEMONS)
        {
            float hp = arenaBehaviour.Arena.Team1.CurrentPokemonBehaviour.Pokemon.EffectiveStats[PokemonStat.HP];
            float hpMax = arenaBehaviour.Arena.Team1.CurrentPokemonBehaviour.Pokemon.EffectiveStats[PokemonStat.HP_MAX];

            Bar.fillAmount = 1 / hpMax * hp;
            healthText.text = hp + "/" + hpMax;
           
        }

        if (arenaBehaviour.Arena.ArenaStage == ArenaStage.FIRST_COMBAT_ANIMATION || arenaBehaviour.Arena.ArenaStage == ArenaStage.SECOND_COMBAT_ANIMATION)
        {
            string attackname = arenaBehaviour.Arena.Team1.PreparedAttackAction.Name;
            AttackName.text =  attackname;
        }
        else
        {
            AttackName.text = "";
    }
        
    }
}
