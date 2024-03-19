using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : CharacterCombat
{

    PlayerMove playerMove;
    CharacterSkills characterSkills;
    List<Skill> skillsList;

    void Start()
    {
        base.Start();
        playerMove = GetComponent<PlayerMove>();
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
                base.selectedSkill = skillsList[input];
            }
        }

        if (PlayerInput.selectedTarget == null || PlayerInput.selectedTarget.CompareTag("Enemy") == false)
            return;

        HandleAttack();
        
    }

    void HandleAttack()
    {

        if (base.selectedSkill == null)
            return;
        if (GetComponent<PlayerState>().state == PlayerState.State.Casting)
        {
            HUD.SetMessageDebug("Você ainda está castando uma skill, aguarde...");
            base.selectedSkill = null;
            return;
        }

        base.selectedSkill.caster = transform;
        base.selectedSkill.target = PlayerInput.selectedTarget;

        base.target = PlayerInput.selectedTarget;

        bool isTargetInCasterRange = base.selectedSkill.IsTargetInCasterRange();
        //bool isSkillReady = combatHandler.selectedSkill.IsSkillReady();

        if (isTargetInCasterRange == false)
        {
            playerMove.followSelectedTarget = true;
        }

        if (base.selectedSkill.CanCastSkill())
        {
            // Para de seguir o inimigo
            playerMove.followSelectedTarget = false;
            // Vira o jogador em direção ao alvo
            Utils.LookAtYZ(transform, base.selectedSkill.target.position);
            // Inicia o trigger da skill
            base.selectedSkill.CastSkill();
            // Alterna os estados de casting para que o jogador pare de se mover
            StartCoroutine(GetComponent<PlayerState>().SwitchStateForDuration(PlayerState.State.Casting, PlayerState.State.Idle, base.selectedSkill.castingTime));
            // Limpa a skill do cache
            base.selectedSkill = null;
        }
    }


}
