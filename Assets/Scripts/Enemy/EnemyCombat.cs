using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{

    public Transform target;
    EnemyMove enemyMove;
    CombatHandler combatHandler;
    CharacterSkills characterSkills;

    public bool isFighting = false;

    Skill selectedSkill;

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
            HandleAttack();
        }

        HandleEnemySkillbar();

    }

    void HandleAttack()
    {

        Skill skill = combatHandler.selectedSkill;

        if (skill == null)
            return;

        bool isAvailableRange = combatHandler.IsAvailableRange(skill, target);
        bool isAvailableSkill = combatHandler.IsAvailableSkill(skill);

        if (isAvailableRange == false)
        {
            enemyMove.followSelectedTarget = true;
        }

        if (isAvailableRange && isAvailableSkill)
        {
            enemyMove.followSelectedTarget = false;
            combatHandler.CastSkill(skill, target);

        }
    }

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
