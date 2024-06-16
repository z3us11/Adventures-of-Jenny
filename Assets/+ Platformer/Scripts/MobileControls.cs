using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public Joystick joystick;
    public Button jumpBtn;
    public Button sprintBtn;

    [HideInInspector]
    public float onScreenXInput;
    [HideInInspector]
    public float onScreenYInput;
    [HideInInspector]
    public bool onScreenJumpBtnPressed = false;
    [HideInInspector]
    public bool onScreenSprintBtnPressed = false;

    private void Update()
    {
        OnJoystickMoved();
    }

    private void OnJoystickMoved()
    {
        if (joystick.Horizontal > 0)
            onScreenXInput = 1;
        else if (joystick.Horizontal < 0)
            onScreenXInput = -1;
        else
            onScreenXInput = 0;

        if (joystick.Vertical > 0)
            onScreenYInput = 1;
        else if (joystick.Vertical < 0)
            onScreenYInput = -1;
        else
            onScreenYInput = 0;
    }

    public void OnPressJumpButton()
    {
        onScreenJumpBtnPressed = true;
    }

    public void OnReleaseJumpButton()
    {
        onScreenJumpBtnPressed = false; 
    }

    public void OnPressSprintButton()
    {
        onScreenSprintBtnPressed = true;
    }

    public void OnReleaseSprintButton()
    {
        onScreenSprintBtnPressed = false;
    }
}
