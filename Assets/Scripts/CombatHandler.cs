using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CombatHandler : MonoBehaviour
{

    public Skill selectedSkill;
    public Transform target;

    [HideInInspector]
    public bool isCasting = false;

    Animator animator;
    CharacterStatus characterStatus;
    Transform damageDisplay; // pensar numa forma otimizada

    void Start()
    {
        animator = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        damageDisplay = GameObject.FindWithTag("Display").transform.Find("Damage").transform;
    }
    
    
    //public bool CastSkill()
    //{

    //    CombatHandler opponentCombatHandler = target.GetComponent<CombatHandler>();

    //    if (opponentCombatHandler == null)
    //        return false;


    //    string animation;
    //    if (selectedSkill.animationClip == null)
    //        animation = selectedSkill.name.ToLower(); // trocar espa�o por underline
    //    else
    //        animation = selectedSkill.animationClip.name;

    //    transform.LookAt(target);
    //    animator.Play(animation);
    //    isCasting = true;
    //    //StartCoroutine( Utils.CheckAnimationCompleted(animator, animation, ()=> { EndSkillCastAnimation(); }));

    //    HUD.SetMessageDebug($"Skill [{selectedSkill.name}] lan�ada!");
    //    selectedSkill.countdownElapsed = selectedSkill.countdown;

    //    if (selectedSkill.abilityPrefab == null)
    //    {
    //        opponentCombatHandler.TakeHitFrom(selectedSkill);
    //        selectedSkill = null;
    //    }

    //    return true;
    //}

    // Called by skill animation event
    //void StartSkillAnimation()
    //{
    //    if (selectedSkill != null && selectedSkill.abilityPrefab != null)
    //    {
    //        Transform instantiated = Instantiate(selectedSkill.abilityPrefab, Path.skillPool);
            
    //        if( selectedSkill.isProjectile)
    //        {
    //            Vector3 startPosition = transform.position;
    //            startPosition.y = transform.GetComponent<Collider>().bounds.size.y / 2;
    //            instantiated.position = startPosition;
    //            instantiated.AddComponent<SkillComponent>();
    //            instantiated.GetComponent<SkillComponent>().skill = selectedSkill;
    //            instantiated.GetComponent<SkillComponent>().skill.target = target;
    //        }
    //        else
    //        {
    //            Vector3 startPosition = target.position;
    //            startPosition.y = transform.GetComponent<Collider>().bounds.size.y / 2;
    //            instantiated.position = startPosition;
    //            Utils.LookAtYZ(instantiated, transform.position);
    //            target.GetComponent<CombatHandler>().TakeDamage(selectedSkill.damage);
    //        }
    //        selectedSkill = null;
    //    }
    //}
    void EndSkillCastAnimation()
    {
        isCasting = false;
    }

    // Rever o conceito dessa fun��o e se ela � relevante ela existir
    public bool TakeHitFrom(Skill skill)
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
