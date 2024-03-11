using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public static List<Skill> skills = new List<Skill>
    {
        new Skill(
            "Punch", // name
            2, // damage
            1.5f, // countdown
            2 // range
        ),
        new Skill(
            "Fireball", // name
            3, // damage
            1.5f, // countdown
            5 // range
        ),
        new Skill(
            "Meteor", // name
            4, // damage
            1.5f, // countdown
            10 // range
        ),
    };

    private void Awake()
    {
        skills[0].basicAttack = true;
    }

    public static Skill FindSkillByName( string name )
    {
        foreach (Skill skill in skills)
        {
            if (skill.name == name)
                return skill;
        }
        return null;
    }

}
