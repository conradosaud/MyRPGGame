using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    public string name = "None";

    [Header("Health")]
    public int currentLife = 100;
    [HideInInspector] public int maximumLife = 100;
    public int currentMana = 50;
    [HideInInspector] public int maximumMana = 50;
    public float regenTimeSeconds = 1.5f;
    public int lifeRegen = 1;
    public int manaRegen = 2;

    [Header("Attack basis")]
    public float range = Constants.rangeDistanceOffset;
    public float attackSpeed = 0.3f;
    public float moveSpeed = 5;

    [Header("Attributes")]
    public Dictionary<string, Dictionary<string, int>> status = new Dictionary<string, Dictionary<string, int>>() {
        { "strength", new Dictionary<string, int>() {
            { "points", 10 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "intelligence", new Dictionary<string, int>() {
            { "points", 10 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "agility", new Dictionary<string, int>() {
            { "points", 10 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "vitality", new Dictionary<string, int>() {
            { "points", 10 },
            { "bonus", 0 },
            { "modifier", 0 }
        }}
    };

    private void Start()
    {
        HUD.UpdateHealthbar();
    }

    public void StartRegen()
    {
        // Check a possibility to stop this
        StartCoroutine(RecoverRegen());
        IEnumerator RecoverRegen()
        {
            yield return new WaitForSeconds(regenTimeSeconds);
            RecoverLifeAndMana(lifeRegen, manaRegen);
            StartRegen();
        }
    }

    public int GetStatus(string statusName)
    {
        return status[statusName]["points"] + status[statusName]["bonus"] + status[statusName]["modifier"];
    }

    public void AddBonus(string statusName, int value, float time)
    {
        StartCoroutine(AddBonus(statusName, value, time, true));
    }
    private IEnumerator AddBonus(string statusName, int value, float time, bool thisClass = true)
    {
        status[statusName]["bonus"] += value;
        yield return new WaitForSeconds(time);
        RemoveBonus(statusName, value);
    }

    public void RemoveBonus(string statusName, int value = -1)
    {
        if( value == -1)
            status[statusName]["bonus"] = 0;
        else
            status[statusName]["bonus"] -= value;

        if( status[statusName]["bonus"] < 0 )
            status[statusName]["bonus"] = 0;
    }

    public void RecoverLifeAndMana( int lifeValue, int manaValue)
    {
        RecoverLife(lifeValue);
        RecoverMana(manaValue);
    }

    public void RecoverLife(int value)
    {

        if (currentLife >= maximumLife)
            return;

        if (value + currentLife > maximumLife)
        {
            value = maximumLife;
        }

        currentLife += value;
        HUD.UpdateLifebar();

    }

    public void RecoverMana( int value )
    {

        if (currentMana >= maximumMana)
            return;

        if( value + currentMana> maximumMana)
        {
            value = maximumMana;
        }

        currentMana+= value;
        HUD.UpdateLifebar();

    }

}
