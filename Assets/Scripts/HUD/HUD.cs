using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public static Transform hud;
    public static Transform messages;
    public static Transform messagesHoveredTarget;
    public static Transform messagesSelectedTarget;
    public static Transform messagesDebug;

    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.FindWithTag("HUD").transform;
        messages = hud.Find("Messages");
        messagesHoveredTarget = messages.Find("HoveredTarget");
        messagesSelectedTarget = messages.Find("SelectedTarget");
        messagesDebug = messages.Find("Debug");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetHoveredTarget( string message)
    {
        messagesHoveredTarget.GetComponent<TextMeshProUGUI>().text = message;
    }
    public static void SetSelectedTarget( string message)
    {
        messagesSelectedTarget.GetComponent<TextMeshProUGUI>().text = message;
    }

    public static void SetMessageDebug( string message)
    {
        messagesDebug.GetComponent<TextMeshProUGUI>().text = message;
    }

}
