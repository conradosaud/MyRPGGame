using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Skill FindSkillByName( List<Skill> skillList, string name )
    {
        foreach (Skill skill in skillList)
        {
            if (skill.name == name)
                return skill;
        }
        return null;
    }
}
