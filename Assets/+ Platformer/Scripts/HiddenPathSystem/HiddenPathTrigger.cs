using DG.Tweening;
using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenPathTrigger : MonoBehaviour
{
    public Tilemap tilemap;
    PlayerController playerController;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerController = collision.transform.parent.GetComponent<PlayerController>();
            if(playerController.canEnterHiddenPath)
            {
                DOTween.ToAlpha(() => tilemap.color, x => tilemap.color = x, 0, 1);
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        DOTween.ToAlpha(() => tilemap.color, x => tilemap.color = x, 1, 1);
    //    }
    //}
}
