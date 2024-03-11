using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public static Vector3 pointClicked;
    public static Transform hoveredTarget;
    public static Transform selectedTarget;

    void Update()
    {
        // Get clicked location
        pointClicked = GetClickedPosition();

        // Get hovered target on mouse hover
        hoveredTarget = GetTarget();
        // Get selected target on click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SelectTarget( hoveredTarget );
        }

        // Debug messages on screen to dev orientation
        ShowDebugDisplays();
    }

    Vector3 GetClickedPosition()
    {

        Vector3 targetPosition = Vector3.zero;

        if (Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                targetPosition = hit.point;
            }
            else
            {
                // Se o ponto atingido estiver fora do terreno (skybox), calcule um ponto na mesma direção
                targetPosition = ray.GetPoint(1000f); // Use um valor grande para garantir que está fora do terreno
            }

            //targetPosition.y = 0;

        }

        return targetPosition;

    }

    Transform GetTarget()
    {

        Transform target = null;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            // Calcula a direção para o inimigo
            target = hit.transform;
        }

        return target;

    }

    void ShowDebugDisplays()
    {
        if (hoveredTarget != null)
            HUD.SetHoveredTarget(hoveredTarget.gameObject.name);

        if (selectedTarget != null)
            HUD.SetSelectedTarget(selectedTarget.gameObject.name);
    }

    void SelectTarget( Transform target)
    {
        // Diselect last selected target
        if( selectedTarget != null && selectedTarget.GetComponent<Outline>() )
            selectedTarget.GetComponent<Outline>().enabled = false;

        if (target == null)
            return;

        // Add new selected target
        selectedTarget = target;
        if( selectedTarget.CompareTag("Enemy"))
        {
            selectedTarget.GetComponent<Outline>().enabled = true;
        }
    }

}
