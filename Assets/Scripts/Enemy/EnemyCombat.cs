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
            HandleAttack();
        }

        HandleEnemySkillbar();

    }

    void HandleAttack()
    {

        if (combatHandler.selectedSkill == null)
            return;
        if (GetComponent<EnemyState>().state == EnemyState.State.Casting)
        {
            combatHandler.selectedSkill = null;
            return;
        }

        combatHandler.selectedSkill.caster = transform;
        combatHandler.selectedSkill.target = combatHandler.target;

        bool isTargetInCasterRange = combatHandler.selectedSkill.IsTargetInCasterRange();

        if (isTargetInCasterRange == false)
        {
            enemyMove.followSelectedTarget = true;
        }

        if (combatHandler.selectedSkill.CanCastSkill())
        {

            enemyMove.followSelectedTarget = false;
            Utils.LookAtYZ(transform, combatHandler.selectedSkill.target.position);
            combatHandler.selectedSkill.CastSkill();
            StartCoroutine(GetComponent<EnemyState>().SwitchStateForDuration(EnemyState.State.Casting, EnemyState.State.Idle, combatHandler.selectedSkill.castingTime));
            combatHandler.selectedSkill = null;

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
