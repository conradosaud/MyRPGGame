using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillbarHandler : MonoBehaviour
{

    Transform hud;
    Transform player;
    CharacterStatus characterStatus;
    List<Skill> skills;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        hud = GameObject.FindWithTag("HUD").transform;

        characterStatus = player.GetComponent<CharacterStatus>();
        skills = player.GetComponent<CharacterSkills>().skills;
    }

    
    private void FixedUpdate()
    {
        // Decides if is a better way call here or inside de "CastSkill" on combat script
        UpdateSkillbar();
    }

    public void UpdateSkillbar()
    {
        for (int i = 0; i < skills.Count; i++)
        {

            Skill skill = skills[i];

            if (skill.countdownElapsed > 0)
            {

                skill.countdownElapsed -= Time.deltaTime;

                // Add a HUD script
                Transform slot = hud.transform.Find("Skillbar").GetChild(i);
                Color color = slot.Find("IconImage").GetComponent<RawImage>().color;
                if (skill.countdownElapsed > 0)
                {
                    color.a = 0.3f;
                    slot.Find("IconImage").GetComponent<RawImage>().color = color;
                    slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().text = skill.countdownElapsed.ToString("0.0");
                    slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    color.a = 1;
                    slot.Find("IconImage").GetComponent<RawImage>().color = color;
                    slot.Find("TextTimer").GetComponent<TextMeshProUGUI>().enabled = false;
                }

            }
        }
    }

}
