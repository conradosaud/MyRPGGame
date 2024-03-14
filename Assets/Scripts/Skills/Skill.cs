using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/New Skill", order = 1)]
[CreateAssetMenu]
public class Skill : ScriptableObject
{

    public string name;
    public int damage;
    public float countdown;
    //[HideInInspector]
    public float countdownElapsed = 0;
    public int range;

    public bool isProjectile = false;
    public bool canMoveOnCast = false;

    public AnimationClip animationClip;
    public Transform abilityPrefab = null;
    [HideInInspector]
    public Transform target = null;


    public static Skill FindSkillByName(List<Skill> skills, string name)
    {
        foreach (Skill skill in skills)
        {
            if (skill.name == name)
                return skill;
        }
        return null;
    }

}
