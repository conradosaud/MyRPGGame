using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{

    public string name = "None";
    public int healthPoints = 100;
    public int manaPoints = 50;
    public float range = Constants.rangeDistanceOffset;
    public float attackSpeed = 0.3f;
    public float moveSpeed = 5;

    public Dictionary<string, Dictionary<string, int>> status = new Dictionary<string, Dictionary<string, int>>();

    private void Start()
    {

        void createNewStatus(string name)
        {
            status[name] = new Dictionary<string, int>();
            status[name]["points"] = 10;
            status[name]["bonus"] = 0;
            status[name]["modifier"] = 0;
        }

        createNewStatus("strength");
        createNewStatus("intelligence");
        createNewStatus("agility");
        createNewStatus("vitality");

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

}
