using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    EnemyMove enemyMove;
    CombatHandler combatHandler;
    CharacterSkills characterSkills;

    public bool isFighting = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyMove = GetComponent<EnemyMove>();
        combatHandler = GetComponent<CombatHandler>();
        characterSkills = GetComponent<CharacterSkills>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( isFighting)
        {
            combatHandler.selectedSkill = GetAvailableSkill();
            //HandleAttack();
        }

        HandleEnemySkillbar();

    }

    //void HandleAttack()
    //{

    //    if (combatHandler.selectedSkill == null)
    //        return;

    //    bool isAvailableRange = combatHandler.IsAvailableRange(combatHandler.selectedSkill);
    //    bool isAvailableSkill = combatHandler.IsAvailableSkill(combatHandler.selectedSkill);

    //    if (isAvailableRange == false)
    //    {
    //        enemyMove.followSelectedTarget = true;
    //    }

    //    if (isAvailableRange && isAvailableSkill)
    //    {
    //        enemyMove.followSelectedTarget = false;
    //        combatHandler.CastSkill();

    //    }
    //}

    Skill GetAvailableSkill()
    {
        foreach( Skill skill in characterSkills.skills)
        {
            if( skill.countdownElapsed <= 0)
            {
                return skill;
            }
        }
        return null;
    }

    void HandleEnemySkillbar()
    {
        foreach( Skill skill in characterSkills.skills)
        {
            if( skill.countdownElapsed > 0)
            {
                skill.countdownElapsed -= Time.deltaTime;
            }
        }
    }

}
