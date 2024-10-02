using Cinemachine;
using DG.Tweening;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour
{
    public PlayerController player;
    public CinemachineVirtualCamera virtualCamera;

    public Image fadeIn;

    private void Start()
    {
        player.canPlayerMove = false;
        FadeOut();
    }

    private void FadeOut()
    {
        fadeIn.DOFade(0, 3f).OnComplete(() =>
        {
            fadeIn.gameObject.SetActive(false);
            virtualCamera.transform.DOMove(player.transform.position, 1f).OnComplete(() =>
            {
                virtualCamera.Follow = player.transform;
                player.canPlayerMove = true;
            });
        });
    }
}
