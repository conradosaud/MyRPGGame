using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{

    Skill skill { get; set; }

    void Initiate();
    void ExecuteCharacterAnimation();
    void ExecuteSkillAnimation();
}
