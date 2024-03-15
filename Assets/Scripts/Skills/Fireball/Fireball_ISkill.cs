using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Fireball_ISkill : MonoBehaviour, ISkill
{

    // Skill casted
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }
    // Caster animator
    Animator casterAnimator;

    // -- Custom skill variables
    public AnimationClip casterAnimationClip;
    public float skillStartTime = 1.5f;
    public float velocity = 3f;
    public float zOffset = 1.5f;
    bool isProjecting = false;

    private void Start()
    {
        if (skill == null)
        {
            Debug.Log("-- Missing skill reference on " + this.name);
            return;
        }
        casterAnimator = skill.caster.GetComponent<Animator>();

        Initiate();

    }

    private void FixedUpdate()
    {
        if (!isProjecting)
            return;

        if (skill.target == null)
            Destroy(gameObject);

        Vector3 moveDirection = Vector3.MoveTowards(transform.position, skill.target.position, velocity * Time.deltaTime);
        moveDirection.y = GetPositionY();
        transform.position = moveDirection;

    }

    void SwitchMeshRender( bool enable)
    {
        GetComponent<Renderer>().enabled = enable;
        GetComponent<Collider>().enabled = enable;
    }

    public float GetPositionY()
    {
        if (skill.caster.GetComponent<Collider>() == null)
            return skill.caster.position.y;
        return skill.caster.GetComponent<Collider>().bounds.size.y / 1.5f;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform == skill.target)
        {
            if (collider.GetComponent<CombatHandler>() != null)
                collider.GetComponent<CombatHandler>().TakeHitFrom(skill);
            Destroy(gameObject);
        }
    }

    // --------- ISkill interface functions ---------

    public void Initiate()
    {
        SwitchMeshRender(false);
        Vector3 newPosition = skill.caster.position;
        newPosition.y = GetPositionY();
        newPosition.z += zOffset;
        transform.position = newPosition;
        Utils.LookAtYZ(transform, skill.target.position);

        // Always call the beginning of the character's animation
        ExecuteCharacterAnimation();
    }

    public void ExecuteCharacterAnimation()
    {
        string animationName = casterAnimationClip.name.Split("_Animation")[0];
        casterAnimator.Play(animationName);
        Invoke("ExecuteSkillAnimation", skillStartTime);
    }

    public void ExecuteSkillAnimation()
    {
        SwitchMeshRender(true);
        isProjecting = true;
    }

}
