using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{

    EnemyCombat enemyCombat;

    void Start()
    {
        //enemyMove = transform.parent.GetComponent<EnemyMove>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if( collider.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemyCombat>().isFighting = true;
            transform.parent.GetComponent<EnemyCombat>().target = GameObject.FindWithTag("Player").transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemyCombat>().isFighting = false;
            transform.parent.GetComponent<EnemyCombat>().target = null;
        }
    }
}
