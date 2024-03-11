using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if( collider.CompareTag("Player"))
        {

        }
    }
}
