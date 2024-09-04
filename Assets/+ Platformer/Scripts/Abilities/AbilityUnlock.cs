using Platformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    public AbilitySO ability;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.transform.parent.GetComponent<PlayerController>();
            playerController.UnlockAbility(ability.abilityType);
            playerController.abilityUnlockPanel.SetupAbilityUnlockPanel(ability);
            gameObject.SetActive(false);
        }
    }
}

public enum AbilityType
{
    LedgeGrab,
    WallJump,
    WallRun,
    Sprint,
    ZoomMap,
    ViewMap,
    EnterHiddenPath
}
