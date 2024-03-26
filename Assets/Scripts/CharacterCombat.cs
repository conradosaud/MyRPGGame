using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterCombat : MonoBehaviour
{

    [HideInInspector]
    public Skill selectedSkill;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool isCasting = false;

    Animator animator;
    CharacterStatus characterStatus;
    Transform damageDisplay; // pensar numa forma otimizada

    protected void Start()
    {
        animator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        damageDisplay = GameObject.FindWithTag("Display").transform.Find("Damage").transform;
    }

    // Rever o conceito dessa fun��o e se ela � relevante ela existir
    public bool TakeHitFrom(Skill skill)
    {
        //TakeDamage(skill.damage);
        return true;
    }

    public void TakeDamage(int damage)
    {
        characterStatus.currentLife -= damage;
        HUD.UpdateLifebar();
        DisplayDamage(damage);
        verifyIsDead();
    }

    public void ConsumeMana(int value)
    {
        characterStatus.currentMana -= value;
        HUD.UpdateManabar();
    }

    void verifyIsDead()
    {
        if (characterStatus.currentLife <= 0)
        {
            HUD.SetMessageDebug($"O alvo [{characterStatus.name}] est� morto");
            Destroy(gameObject);
        }
    }

    void DisplayDamage(int value)
    {
        Transform instance = Instantiate(damageDisplay, transform);
        instance.gameObject.SetActive(true);
        instance.Find("Value").GetComponent<TextMeshProUGUI>().text = value.ToString();
        Vector3 position = transform.position;
        position.y = transform.GetComponent<Collider>().bounds.size.y;
        instance.transform.position = position;
    }

}
