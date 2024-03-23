using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class ButterflyBlow_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    // -- Custom skill variables
    public string skillAnimationName;

    public float skillStartTime = 0.2f;
    public float firstDamageTime = 0.2f;
    public float secondDamageTime = 0.6f;

    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        // Initiate this skill configs
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
        transform.position = SkillUtilities.GetTargetCenterPosition(skill);
        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeDamage(skill.GetDamage());
    }


}
