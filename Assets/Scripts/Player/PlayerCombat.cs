using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public static bool isFighting = false;
    public static bool canAttack = true;

    CharacterStatus characterStatus;
    CombatHandler combatHandler;
    CharacterSkills characterSkills;
    List<Skill> skillsList;

    // Stored last skill pressed to cast when its available
    //public static Skill selectedSkill = null;

    void Start()
    {
        characterStatus = GetComponent<CharacterStatus>();
        combatHandler = GetComponent<CombatHandler>();
        characterSkills = GetComponent<CharacterSkills>();
        skillsList = characterSkills.skills;
    }

    void Update()
    {

        // Get keys to defined if is a casting
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int input = i - 1;
                combatHandler.selectedSkill = skillsList[input];
            }
        }

        if (PlayerInput.selectedTarget == null || PlayerInput.selectedTarget.CompareTag("Enemy") == false)
            return;

        HandleAttack();
        
    }

    void HandleAttack()
    {

        if (combatHandler.selectedSkill == null)
            return;

        combatHandler.selectedSkill.caster = transform;
        combatHandler.selectedSkill.target = PlayerInput.selectedTarget;

        combatHandler.target = PlayerInput.selectedTarget;

        bool isAvailableRange = combatHandler.selectedSkill.IsTargetInCasterRange();
        bool isAvailableSkill = combatHandler.selectedSkill.IsSkillReady();

        if (isAvailableRange == false)
        {
            PlayerMove.followSelectedTarget = true;
        }

        if (isAvailableRange && isAvailableSkill)
        {
            combatHandler.selectedSkill.CastSkill();
            PlayerMove.followSelectedTarget = false;
            combatHandler.selectedSkill = null;
        }
    }


}
