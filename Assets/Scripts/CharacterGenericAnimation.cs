using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenericAnimation : MonoBehaviour
{

    public bool detectMove = true;
    public float velocityToRun = 0.9f;

    Animator animator;
    Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        if( detectMove )
            rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!detectMove)
            return;

        animator.SetBool("run", rb.velocity.magnitude > velocityToRun);

    }

    public void Attack()
    {
        animator.SetTrigger("magic");
    }

    public void StartCastAnimation(Skill skill)
    {
        animator.SetTrigger(skill.animation);
    }

    public void SkillCasted( Skill skill )
    {

    }

}
