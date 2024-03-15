using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // Método genérico para encontrar uma habilidade com base na interface ISkill
    public static T GetSkillComponent<T>(Transform skillPrefab) where T : MonoBehaviour, ISkill
    {
        T skillComponent = skillPrefab.GetComponent<T>();
        if (skillComponent == null)
        {
            Debug.LogError($"Skill component of type {typeof(T).Name} not found on {skillPrefab.name}");
        }
        return skillComponent;
    }

}
