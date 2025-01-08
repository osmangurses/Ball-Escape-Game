using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;
    public float x_input;
    public Button x_negative, x_positive;
    bool jump = false;
    bool isMobile;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (OS_Input_System.instance.device_type != "desktop")
        {
            isMobile = true;
        }
    }
    
    private void Update()
    {
        if (!isMobile) 
        {
            x_input = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Vertical"))
            {
                jump = true;
            }
        }
    }
    public void SetJump(bool isJump)
    {
        jump=isJump;
    }
    public bool GetJump()
    {
        var temp = jump;
        jump=false;
        return temp;
    }
    public void IncreaseXInput()
    {
        x_input = 1;
    }
    public void DecreaseXInput()
    {
        x_input = -1;
    }
    public void NotrXInput()
    {
        x_input=0;
    }
}
