using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{

    EnemyCombat enemyCombat;
    CombatHandler combatHandler;

    void Start()
    {
        enemyCombat = transform.GetComponent<EnemyCombat>();
        combatHandler = transform.GetComponent<CombatHandler>();
    }

    private void OnTriggerStay(Collider collider)
    {

        // Prevent bug. I dont know why what is exactly
        if (enemyCombat == null)
            return;

        if( collider.CompareTag("Player"))
        {
            enemyCombat.isFighting = true;
            combatHandler.target = collider.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (enemyCombat == null)
            return;

        if ( other.CompareTag("Player"))
        {
            enemyCombat.isFighting = false;
            combatHandler.target = null;
        }
    }
}
