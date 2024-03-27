using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class IvigoratingBooty_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    public float minimunLifeCondition = 0.30f;
    public float recoveryLifePercent = 0.025f;

    bool isSkillAvailable = true;

    CharacterStatus characterStatus;

    private void Start()
    {
        characterStatus = skill.caster.GetComponent<CharacterStatus>();
        transform.position = SkillUtilities.GetCasterCenterPosition(skill);
        ExecuteSkillAnimation();
    }

    public void ExecuteSkillAnimation()
    {

        GetComponent<ParticleSystem>().Play();

        int lifePercent = (int) Math.Floor(characterStatus.maximumLife * recoveryLifePercent);
        int intelligencePercent = skill.GetDamage();
        int finalRecovery = lifePercent + intelligencePercent;

        Debug.Log("Recovering passive");

        characterStatus.RecoverLife(finalRecovery);

    }

}
