using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkills : MonoBehaviour
{

    public List<Skill> skills = new List<Skill>();
    public Skill basicAttack = null;

    private void Start()
    {
        skills.Add(Skills.FindSkillByName("Punch"));
        skills.Add(Skills.FindSkillByName("Fireball"));
        skills.Add(Skills.FindSkillByName("Meteor"));

        basicAttack = Utils.FindSkillByName(skills, "Punch");

    }

}
