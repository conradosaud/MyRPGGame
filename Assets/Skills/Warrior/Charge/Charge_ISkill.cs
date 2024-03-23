using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Charge_ISkill : MonoBehaviour, ISkill
{

    // Skill interface refering to original casted skill
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }

    public float skillStartTime = 1f;

    Rigidbody rb;
    bool startCharge = false;


    private void Start()
    {

        // Check skill for prevent bugs
        if ( SkillUtilities.isSkillNull(skill) == false)
            return;
        SkillUtilities.ShowHUDCastMessage(skill);

        // Initiate this skill configs
        ExecuteCharacterAnimation();

    }

    private void FixedUpdate()
    {
        if (startCharge)
        {

            if (Vector3.Distance(skill.caster.position, skill.target.position) < 2f)
            {
                EndCharge();
                return;
            }

            rb = skill.caster.GetComponent<Rigidbody>();
            PlayerMove move = skill.caster.GetComponent<PlayerMove>();
            CharacterStatus characterStatus = skill.caster.GetComponent<CharacterStatus>();
            //rb.velocity = Vector3.MoveTowards( skill.caster.position, skill.target.position,  characterStatus.moveSpeed + skill.velocity * Time.deltaTime);

            Vector3 moveDirection = skill.target.position;
            // Look to destiny point
            Utils.LookAtYZ(transform, moveDirection);
            moveDirection = (moveDirection - skill.caster.position).normalized;

            // Apply velocity to the axis
            moveDirection.x *= characterStatus.moveSpeed + skill.velocity;
            moveDirection.z *= characterStatus.moveSpeed + skill.velocity;

            // Update the velocity of the rigidbody
            Vector3 velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            rb.velocity = velocity;
        }
    }


    public void ExecuteCharacterAnimation()
    {
        // Play the animation by its name in caster animator
        skill.caster.GetComponent<Animator>().Play(skill.name);
        // Wait characters animation time to executes the skill animation
        Invoke("ExecuteSkillAnimation", skillStartTime);
    }

    public void ExecuteSkillAnimation()
    {

        // Cancel and destroy object if there's target anymore
        if (skill.target == null)
        {
            Destroy(gameObject);
            return;
        }

        StartCharge();
        //Invoke("StartCharge", 1.5f);
        

    }

    void StartCharge()
    {
        transform.GetComponent<Collider>().bounds.Encapsulate(skill.caster.GetComponent<Collider>().bounds);
        startCharge = true;
    }

    void EndCharge()
    {
        startCharge = false;
        skill.caster.GetComponent<Animator>().Play("Idle");
        // TODO : AJUSTAR ISSO PARA QUANDO ACERTAR O ALVO APENAS
        SkillDamage();
    }

    void SkillDamage()
    {
        transform.position = SkillUtilities.GetTargetCenterPosition(skill);
        // Apply this skill effect
        if (skill.target.GetComponent<CharacterCombat>() != null)
            skill.target.GetComponent<CharacterCombat>().TakeDamage(skill.GetDamage());
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("opae");
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("opae22222");
    }

}
