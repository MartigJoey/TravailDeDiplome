using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityController;


public class Script1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityCommands.StartServer("008");
    }

    // Update is called once per frame
    void Update()
    {
        UnityCommands.ReceiveMessage();
    }
}