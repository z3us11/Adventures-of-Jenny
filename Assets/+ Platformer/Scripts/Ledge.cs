using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.transform.parent.GetComponent<PlayerController>();
            playerController.IsTouchingLedge = true;
            playerController.CurrentLedge = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.transform.parent.GetComponent<PlayerController>();
            playerController.IsTouchingLedge = false;
            playerController.CurrentLedge = null;
        }
    }
}
