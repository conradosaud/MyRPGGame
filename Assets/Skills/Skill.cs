using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Custom Item", menuName = "New Skill", order = 2)]
public class Skill : ScriptableObject
{

    public string name = "Skill";
    public int damage = 1;
    public float countdown = 1;
    public int range = 2;
    public float castingTime = 0;

    //[HideInInspector]
    public float countdownElapsed = 0;
    [HideInInspector]
    public Transform caster = null;


    public bool isProjectile = false;
    public bool canMoveOnCast = false;

    //public AnimationClip animationClip;
    public Transform skillPrefab = null;
    [HideInInspector]
    public Transform target = null;

    // Check if the target is in the of caster
    public bool IsTargetInCasterRange()
    {

        if (target == null || caster == null)
        {
            HUD.SetMessageDebug($"Verifque o alvo ou caster de [{name}]");
            return false;
        }

        float distance = Vector3.Distance(caster.position, target.position);

        if( Constants.DEV && caster.CompareTag("Player") )
            HUD.SetMessageDebug($"Skill [{name}] fora de alcance — Distancia [{distance.ToString("0.0")}] | Alcance {range}");
        
        return distance <= range;

    }

    // Return if skill countdown is minus or equals to zero
    public bool IsSkillReady()
    {

        bool isReady = countdownElapsed <= 0;
        
        if (Constants.DEV && !isReady)
            HUD.SetMessageDebug($"Skill [{name}] em recarga, {countdownElapsed.ToString("0.0")} de {countdown}");

        return isReady;

    }

    // Check if range and countdown is OK to be casted
    public bool CanCastSkill()
    {
        return IsTargetInCasterRange() && IsSkillReady();
    }

    // Uses the skill
    public void CastSkill()
    {
        Transform instantiated = Instantiate( skillPrefab, Path.skillPool );
        instantiated.GetComponent<ISkill>().skill = this;
        countdownElapsed = countdown;
    }

}
