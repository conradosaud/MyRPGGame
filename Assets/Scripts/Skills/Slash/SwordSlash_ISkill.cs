using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class SwordSlash_ISkill : MonoBehaviour, ISkill
{

    // Skill casted
    Skill skill;
    Skill ISkill.skill { get { return skill; } set { skill = value; } }
    // Caster animator
    Animator casterAnimator;
    // SkillPrefab instance (for not overrides the original)
    Skill skillPrefab;

    // -- Custom skill variables
    public AnimationClip casterAnimationClip;
    public float skillStartTime = 1.5f;
    public float velocity = 3f;
    public float zOffset = 1.5f;

    private void Start()
    {
        if (skill == null)
        {
            HUD.SetMessageDebug("-- Missing skill reference on " + this.name);
            return;
        }
        else
        {
            if( skill.caster.CompareTag("Player") )
                HUD.SetMessageDebug($"-- Casting skill [{skill.name}] on [target {skill.target.name}]");
        }
        casterAnimator = skill.caster.GetComponent<Animator>();
        //skillPrefab = skill.skillPrefab.Clone();

        Initiate();

    }

    public float GetPositionY()
    {
        if (skill.caster.GetComponent<Collider>() == null)
            return skill.caster.position.y;
        return skill.caster.GetComponent<Collider>().bounds.size.y / 1.5f;
    }

    // --------- ISkill interface functions ---------

    public void Initiate()
    {

        Vector3 newPosition = skill.caster.position;
        newPosition.y = GetPositionY();
        newPosition.z += zOffset;
        transform.position = newPosition;

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

        if (skill.target == null)
        {
            Destroy(gameObject);
            return;
        }

        GetComponent<ParticleSystem>().Play();

        if (skill.target.GetComponent<CombatHandler>() != null)
            skill.target.GetComponent<CombatHandler>().TakeHitFrom(skill);

    }

}
