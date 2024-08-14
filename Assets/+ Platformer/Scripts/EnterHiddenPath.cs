using DG.Tweening;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class EnterHiddenPath : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance.isNight)
                return;

            GameManager.instance.isInHiddenPath = true;

            DOTween.To(() => GameManager.instance.globalLight.color, x => GameManager.instance.globalLight.color = x, GameManager.instance.insideColor, 1);

            Light2D playerLight = collision.transform.parent.GetComponent<PlayerController>().playerLight;
            DOTween.ToAlpha(() => playerLight.color, x => playerLight.color = x, 1, 1);
            GameManager.instance.skyColor.SetColor("_TopColor", GameManager.instance.insideSkyTopColor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance.isNight)
                return;
            if(GameManager.instance.timeChanging &&  !GameManager.instance.isNight)
                return;

            GameManager.instance.isInHiddenPath = false;

            DOTween.To(() => GameManager.instance.globalLight.color, x => GameManager.instance.globalLight.color = x, GameManager.instance.outsideColor, 1);

            Light2D playerLight = collision.transform.parent.GetComponent<PlayerController>().playerLight;
            DOTween.ToAlpha(() => playerLight.color, x => playerLight.color = x, 0, 1);
            GameManager.instance.skyColor.SetColor("_TopColor", GameManager.instance.daySkyTopColor);
        }
    }
}
