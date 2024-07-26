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
    public Light2D globalLight;

    public Color outsideColor;
    public Color insideColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, insideColor, 1);

            Light2D playerLight = collision.transform.parent.GetComponent<PlayerController>().playerLight;
            DOTween.ToAlpha(() => playerLight.color, x => playerLight.color = x, 1, 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, outsideColor, 1);

            Light2D playerLight = collision.transform.parent.GetComponent<PlayerController>().playerLight;
            DOTween.ToAlpha(() => playerLight.color, x => playerLight.color = x, 0, 1);
        }
    }
}
