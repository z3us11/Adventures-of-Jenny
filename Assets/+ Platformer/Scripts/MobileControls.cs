using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public Joystick joystick;
    public Button dpadUp;
    public Button dpadDown;
    public Button dpadLeft;
    public Button dpadRight;
    public Button jumpBtn;
    public Button sprintBtn;

    [Header("Public Variables")]
    public float onScreenXInput;
    public float onScreenYInput;
    public bool onScreenJumpBtnPressed = false;
    public bool onScreenSprintBtnPressed = false;

    public enum DPadButton
    {
        Up,
        Down, 
        Left, 
        Right
    }

    private void Update()
    {
        //OnJoystickMoved(); 
    }

    public void OnToggleMobileControls(Toggle mobileControlsToggle)
    {
        gameObject.SetActive(mobileControlsToggle.isOn);

        joystick.gameObject.SetActive(mobileControlsToggle.isOn);
        jumpBtn.gameObject.SetActive(mobileControlsToggle.isOn);
        sprintBtn.gameObject.SetActive(mobileControlsToggle.isOn);
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

    public void OnPressDPadBtn(int dPadButton)
    {
        if (dPadButton == (int)DPadButton.Right)
            onScreenXInput = 1;
        else if (dPadButton == (int)DPadButton.Left)
            onScreenXInput = -1;
        else if (dPadButton == (int)DPadButton.Up)
            onScreenYInput = 1;
        else if(dPadButton == (int)DPadButton.Down)
            onScreenYInput = -1;
    }

    public void OnReleaseDPadBtn()
    {
        onScreenXInput = 0;
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
