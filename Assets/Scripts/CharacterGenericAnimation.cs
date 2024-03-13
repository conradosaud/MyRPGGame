using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenericAnimation : MonoBehaviour
{

    public bool detectMove = true;
    public float velocityToRun = 0.9f;

    Animator animator;
    CharacterController cc;

    void Start()
    {
        animator = GetComponent<Animator>();
        if( detectMove )
            cc = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (!detectMove)
            return;

        animator.SetBool("run", cc.velocity.magnitude > velocityToRun);

    }

    public void Attack()
    {
        animator.SetTrigger("magic");
    }

}
