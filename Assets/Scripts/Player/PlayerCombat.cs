using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCombat : MonoBehaviour
{

    public static bool isFighting = false;
    public static bool canAttack = true;

    CharacterStatus characterStatus;
    CombatHandler combatHandler;
    CharacterSkills characterSkills;
    List<Skill> skillsList;

    // Stored last skill pressed to cast when its available
    public static Skill selectedSkill = null;

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
                int input = i - 1; // pressed input index
                selectedSkill = skillsList[input];
            }
        }

        if (PlayerInput.selectedTarget == null || PlayerInput.selectedTarget.CompareTag("Enemy") == false)
            return;


        if (selectedSkill == null)
            if (Input.GetKey(KeyCode.Mouse0))
                selectedSkill = characterSkills.basicAttack;
            else
                selectedSkill = null;

        HandleAttack();

        
    }

    void HandleAttack( Skill skill = null )
    {

        if (skill == null)
            skill = selectedSkill;
        if (skill == null)
            return;

        // Persist on follow if basic attack is holding mouse button
        if( skill == characterSkills.basicAttack)
        {
            if( Input.GetKey(KeyCode.Mouse0) == false)
            {
                HandleFollowSelectedTargetToAttack(false);
                return;
            }

        }

        bool isAvailableSkill = combatHandler.IsAvailableSkill(skill);
        bool isAvailableRange = combatHandler.IsAvailableRange(skill);

        if (isAvailableRange == false)
        {
            HandleFollowSelectedTargetToAttack(true, skill);
        }

        if (isAvailableSkill && isAvailableRange && canAttack)
        {

            combatHandler.CastSkill(skill, PlayerInput.selectedTarget);
            HandleFollowSelectedTargetToAttack(false);

        }
    }

    void HandleFollowSelectedTargetToAttack( bool follow, Skill skill = null )
    {
        if( follow)
        {
            selectedSkill = skill;
            PlayerMove.followSelectedTarget = true;
        }
        else
        {
            selectedSkill = null;
            PlayerMove.followSelectedTarget = false;
        }
    }


}
