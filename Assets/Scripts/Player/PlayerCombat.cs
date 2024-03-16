using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class PlayerCombat : MonoBehaviour
{

    public static bool isFighting = false;
    public static bool canAttack = true;

    PlayerMove playerMove;
    CharacterStatus characterStatus;
    CombatHandler combatHandler;
    CharacterSkills characterSkills;
    List<Skill> skillsList;

    // Stored last skill pressed to cast when its available
    //public static Skill selectedSkill = null;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
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
        if (GetComponent<PlayerState>().state == PlayerState.State.Casting)
        {
            HUD.SetMessageDebug("Você ainda está castando uma skill, aguarde...");
            combatHandler.selectedSkill = null;
            return;
        }

        combatHandler.selectedSkill.caster = transform;
        combatHandler.selectedSkill.target = PlayerInput.selectedTarget;

        combatHandler.target = PlayerInput.selectedTarget;

        bool isTargetInCasterRange = combatHandler.selectedSkill.IsTargetInCasterRange();
        //bool isSkillReady = combatHandler.selectedSkill.IsSkillReady();

        if (isTargetInCasterRange == false)
        {
            playerMove.followSelectedTarget = true;
        }

        if (combatHandler.selectedSkill.CanCastSkill())
        {
            Utils.LookAtYZ(transform, combatHandler.selectedSkill.target.position);
            combatHandler.selectedSkill.CastSkill();
            playerMove.followSelectedTarget = false;
            StartCoroutine(GetComponent<PlayerState>().SwitchStateForDuration(PlayerState.State.Casting, PlayerState.State.Idle, combatHandler.selectedSkill.castingTime));
            combatHandler.selectedSkill = null;
        }
    }


}
