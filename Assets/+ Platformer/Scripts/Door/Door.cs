using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public Transform doorBottom;
    public Ease ease;

    public void OpenDoor()
    {
        StartCoroutine(StartOpenDoor());
    }

    IEnumerator StartOpenDoor()
    {
        AudioManager.instance.PlayDoorOpenSound();

        doorBottom.DOLocalMoveY(1.15f, 0.25f).SetEase(ease);
        yield return new WaitForSeconds(0);
    }
}
