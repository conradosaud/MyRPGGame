using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{

    public Skill selectedSkill;

    CharacterStatus characterStatus;
    Transform damageDisplay; // pensar numa forma otimizada

    void Start()
    {
        characterStatus = GetComponent<CharacterStatus>();
        damageDisplay = GameObject.FindWithTag("Display").transform.Find("Damage").transform;
    }

    public bool IsAvailableSkill(Skill skill)
    {

        if (skill.countdownElapsed > 0)
        {
            HUD.SetMessageDebug($"Skill [{skill.name}] em recarga, {skill.countdownElapsed.ToString("0.0")} de {skill.countdown}");
            return false;
        }

        return true;

    }

    public bool IsAvailableRange(Skill skill, Transform target)
    {
        if (target == null)
            return false;

        float distance = Vector3.Distance(transform.position, target.position);
        if ( distance <= skill.range )
        {
            return true;
        }

        HUD.SetMessageDebug($"Skill [{skill.name}] fora de alcance — Distancia [{distance.ToString("0.0")}] | Alcance {skill.range}");
        return false;

    }

    public bool CastSkill(Skill skill, Transform target)
    {

        CombatHandler opponentCombatHandler = target.GetComponent<CombatHandler>();

        if (opponentCombatHandler == null)
            return false;

        opponentCombatHandler.TakeHitFromOther(skill, transform);

        HUD.SetMessageDebug($"Skill [{skill.name}] lançada!");
        skill.countdownElapsed = skill.countdown;

        selectedSkill = null;

        return true;
    }

    // Rever o conceito dessa função e se ela é relevante ela existir
    public bool TakeHitFromOther(Skill skill, Transform other)
    {
        TakeDamage(skill.damage);
        return true;
    }

    public void TakeDamage(int damage)
    {
        characterStatus.healthPoints -= damage;
        DisplayDamage(damage);

        verifyIsDead();

    }

    void verifyIsDead()
    {
        if (characterStatus.healthPoints <= 0)
        {
            HUD.SetMessageDebug($"O alvo [{characterStatus.name}] está morto");
            Destroy(gameObject);
        }
    }

    void DisplayDamage(int value)
    {
        Transform instance = Instantiate(damageDisplay, transform);
        instance.gameObject.SetActive(true);
        instance.Find("Value").GetComponent<TextMeshProUGUI>().text = value.ToString();

    }

}
