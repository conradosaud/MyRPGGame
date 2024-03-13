using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericProjectile : MonoBehaviour
{

    public float velocity = 1.5f;
    Transform target;
    Skill skill;
    //public dynamic callback;

    private void Start()
    {
        if (GetComponent<SkillComponent>() == null)
            return;
        skill = GetComponent<SkillComponent>().skill;

        target = skill.target;
        if (target == null)
            Debug.Log("Missing destiny");
        transform.LookAt(target);
    }

    private void FixedUpdate()
    {
        if (target == null)
            Destroy(gameObject);

        //Vector3 moveDirection = (destiny.position - transform.position).normalized;
        //moveDirection = moveDirection * velocity * Time.deltaTime;
        //transform.Translate(moveDirection);
        Vector3 moveDirection = Vector3.MoveTowards(transform.position, target.position, velocity * Time.deltaTime);
        transform.position = moveDirection;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if( collider.transform == target)
        {
            if (collider.GetComponent<CombatHandler>() != null)
                collider.GetComponent<CombatHandler>().TakeHitFrom( skill );
            Destroy(gameObject);
        }
    }

}
