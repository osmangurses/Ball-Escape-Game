using UnityEngine;

public class OS_Input_System : MonoBehaviour
{

    public static OS_Input_System instance;
    public GameObject UI_control_panel;
    public string device_type;

    private void Awake()
    {
        instance = this;
        if (!Application.isMobilePlatform)
        {
            device_type = "desktop";
        }
        else
        {
            device_type = "mobile";
        }

        if (device_type != "desktop")
        {
            UI_control_panel.SetActive(true);
        }
        else
        {
            UI_control_panel.SetActive(false);
        }
    }
}
