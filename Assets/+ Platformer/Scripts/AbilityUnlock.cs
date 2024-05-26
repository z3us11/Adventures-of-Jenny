using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    public AbilityType abilityType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.transform.parent.GetComponent<PlayerController>();
            playerController.UnlockAbility(abilityType);
            gameObject.SetActive(false);
        }
    }
}

public enum AbilityType
{
    LedgeGrab,
    WallJump,
    WallRun,
    Sprint
}
