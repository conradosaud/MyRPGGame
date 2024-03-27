using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    public string name = "None";

    public int level = 1;
    [SerializeField] float experienceAccumulated = 0;
    public float experienceLoot = 120;

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
            { "points", 100 },
            { "bonus", 0 },
            { "modifier", 0 }
        }},
        { "vitality", new Dictionary<string, int>() {
            { "points", 10 },
            { "bonus", 0 },
            { "modifier", 0 }
        }}
    };

    private Dictionary<int, int> experiencePerLevelTable = new Dictionary<int, int>()
    {
        { 1, 0 },    // Nível 1 requer 0 de experiência
        { 2, 100 },  // Nível 2 requer 100 de experiência
        { 3, 200 },  // Nível 3 requer 200 de experiência
        // Adicione mais níveis e experiência conforme necessário
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
            value = maximumLife - currentLife;
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

    public void EarnExperience(float value)
    {
        // Adiciona a quantidade de experiência ganha à variável experience
        experienceAccumulated += value;

        // Verifica se o personagem alcançou ou ultrapassou a quantidade de experiência necessária para o próximo nível
        while (experienceAccumulated >= ExperienceToNextLevel(level))
        {
            // Incrementa o nível do personagem
            level++;
            Debug.Log("Subiu de level");

            // Se o próximo nível não estiver na tabela de experiência, encerra o loop
            if (!experiencePerLevelTable.ContainsKey(level))
            {
                break;
            }

            // Subtrai a experiência necessária para alcançar o próximo nível da experiência atual
            experienceAccumulated -= experiencePerLevelTable[level];
        }
    }

    private int ExperienceToNextLevel(int level)
    {
        // Se o nível estiver na tabela de experiência, retorna a quantidade de experiência necessária
        if (experiencePerLevelTable.ContainsKey(level))
        {
            return experiencePerLevelTable[level];
        }
        // Se o nível não estiver na tabela, retorna um valor padrão (ou lida com o erro de alguma outra forma)
        else
        {
            Debug.LogWarning("Nível não encontrado na tabela de experiência. Nível: " + level);
            return -1; // Ou outro valor padrão de sua escolha
        }
    }

}
