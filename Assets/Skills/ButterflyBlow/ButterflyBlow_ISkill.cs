using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class ButterflyBlow_ISkill : MonoBehaviour, ISkill
{

    // Skill casted
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    // -- Custom skill variables]
    public string skillAnimationName;

    public int baseDamage = 20;
    public float physicalScale = 35;
    float finalDamage;

    public float skillStartTime = 0.2f;
    public float firstDamageTime = 0.2f;
    public float secondDamageTime = 0.6f;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        /*
         * TODO:
         * Adicionar dano mínimo e máximo
         * Remover initiate da interface
         * Tentar resumir melhor todos os arquivos de uma skill
         * - ver se é possível fazer isso tudo dentro de skill diretamente
         */

        int strengthValue = skill.caster.GetComponent<CharacterStatus>().GetStatus("strength");
        float strengthDamage = strengthValue * (physicalScale / 100);
        finalDamage = baseDamage + strengthDamage;
        finalDamage = (float)Math.Floor(finalDamage);

        // Initiate this skill configs
        Initiate();

    }


    // --------- - ISkill interface functions - ---------

    public void Initiate()
    {

        // Positionate this prefab on caster center
        transform.position = SkillUtilities.GetTargetCenterPosition(skill);

        // Always call the beginning of the character's animation
        ExecuteCharacterAnimation();
    }

    public void ExecuteCharacterAnimation()
    {
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play(skillAnimationName);
        // Wait characters animation time to executes the skill animation
        Invoke("ExecuteSkillAnimation", skillStartTime);
    }

    public void ExecuteSkillAnimation()
    {

        // Cancel and destroy object if there's target anymore
        if (skill.target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Start the particles system. If it has a mesh, shoud be enabled here
        //GetComponent<ParticleSystem>().Play();
        Invoke("SkillDamage", firstDamageTime);
        Invoke("SkillDamage", firstDamageTime + secondDamageTime);

    }

    void SkillDamage()
    {
        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeDamage((int)finalDamage);
    }

}
