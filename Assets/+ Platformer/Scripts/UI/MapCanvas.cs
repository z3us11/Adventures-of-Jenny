using Cinemachine;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCanvas : MonoBehaviour
{
    public CinemachineVirtualCamera vCamera;
    public float cameraMoveSpeed;
    public float cameraZoomSpeed;
    [SerializeField] public Button[] movementButtons;
    [SerializeField] public Button[] zoomButtons;


    PlayerController player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void MoveMap(int index)
    {
        vCamera.Follow = null;
        if(index == 0)
        {
            vCamera.transform.position += Vector3.left * cameraMoveSpeed;
        }
        else if(index == 1)
        {
            vCamera.transform.position += Vector3.right * cameraMoveSpeed;

        }
        else if(index == 2)
        {
            vCamera.transform.position += Vector3.up * cameraMoveSpeed;

        }
        else if(index == 3)
        {
            vCamera.transform.position += Vector3.down * cameraMoveSpeed;

        }
    }

    public void MovingStopped()
    {
        vCamera.Follow = player.transform;
    }

    public void ZoomMap(bool zoomingIn)
    {
        if(zoomingIn)
        {
            if(vCamera.m_Lens.OrthographicSize > 5)
                vCamera.m_Lens.OrthographicSize -= cameraZoomSpeed;
        }
        else
        {
            if(vCamera.m_Lens.OrthographicSize < 15)
                vCamera.m_Lens.OrthographicSize += cameraZoomSpeed;
        }
    }
}
