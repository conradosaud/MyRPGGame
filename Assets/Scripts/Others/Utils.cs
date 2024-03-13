using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public static void LookAtYZ( Transform transform, Vector3 lookAtPosition)
    {
        float originalX = transform.rotation.eulerAngles.x;
        transform.LookAt(lookAtPosition);
        transform.rotation = Quaternion.Euler(originalX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        //return transform.rotation;
    }

}
