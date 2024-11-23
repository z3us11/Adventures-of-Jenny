using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assists : MonoBehaviour
{
    public Button assistsEnableBtn;
    [Space]
    public GameObject assistsPanel;
    [Space]
    public Toggle ledgeGrabToggle;
    public Toggle wallJumpToggle;
    public Toggle sprintToggle;
    public Toggle wallRunToggle;
    public Toggle zoomToggle;
    public Toggle mapToggle;
    public Toggle enterHiddenPathToggle;
    [Space]
    public PlayerController playerController;

    private void Start()
    {
        assistsEnableBtn.onClick.AddListener(ShowHideAssists);

    }

    void ShowHideAssists()
    {
        assistsPanel.SetActive(!assistsPanel.activeSelf);
    }

    public void OnLedgeGrabToggle()
    {
        playerController.canLedgeGrab = ledgeGrabToggle.isOn;
    }

    public void OnWallJumpToggle()
    {
        playerController.canWallJump = wallJumpToggle.isOn;

    }

    public void OnSprintToggle()
    {
        playerController.canSprint = sprintToggle.isOn;
    }

    public void OnWallRunToggle()
    {
        playerController.canWallRun = wallRunToggle.isOn;
    }

    public void OnZoomToggle()
    {
        playerController.canZoomMap = zoomToggle.isOn;
    }

    public void OnMapToggle()
    {
        playerController.canViewMap = mapToggle.isOn;
    }

    public void OnEnterHiddenPathToggle()
    {
        playerController.canEnterHiddenPath = enterHiddenPathToggle.isOn;
    }

    private void Update()
    {
        ledgeGrabToggle.isOn = playerController.canLedgeGrab;
        wallJumpToggle.isOn = playerController.canWallJump;
        sprintToggle.isOn = playerController.canSprint;
        wallRunToggle.isOn = playerController.canWallRun;
        zoomToggle.isOn = playerController.canZoomMap;
        mapToggle.isOn = playerController.canViewMap;
        enterHiddenPathToggle.isOn = playerController.canEnterHiddenPath;
    }
}
