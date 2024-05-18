using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class WakeUpCanvas : MonoBehaviour
{
    
    public GameObject canvas;
    public Boolean canvasState;


    void Start()
    {
        canvasState = false;
        canvas.SetActive(canvasState);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            canvasState = (!canvasState);
            canvas.SetActive(canvasState);
        }
    }
}
