using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float playerPosBeforeVerticalMove;
    [SerializeField] float playerPosBeforeHorizontalMove;
    [SerializeField] float cameraMoveSpeed;

    [SerializeField] bool canMoveUp;
    [SerializeField] bool canMoveLeft;

    DateTime oldTime;
    DateTime currentTime;

    bool isMoving = false;
    bool timerStarted = false;

    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (player.position.y > playerPosBeforeVerticalMove)
        {
            isMoving = true;
        }

        if (player.position.x <= -playerPosBeforeHorizontalMove)
        {
            Debug.Break();
            canMoveLeft = true;
        }
        else
        {
            if(player.position.x > -16.65f)
            {
                canMoveLeft = false;
            }
        }


        if(canMoveUp)
            MoveUp();
        if (canMoveLeft)
        {
            StartCoroutine(MoveCamLeft());
        }

        FollowPlayer();
    }

    void MoveUp()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.up * cameraMoveSpeed);
            if (!timerStarted)
            {
                StartTimer();
                timerStarted = true;
            }
            UpdateMoveSpeed();
        }
        
    }
    IEnumerator MoveCamLeft()
    {
        yield return null ;
        { transform.Translate(Vector2.left * cameraMoveSpeed); }

    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (player.position.y > playerPosBeforeVerticalMove && player.position.y > transform.position.y)
        {
            transform.position = Vector2.Lerp(transform.position, player.position, distance * cameraMoveSpeed * 2 * Time.deltaTime);
            transform.position = new Vector3(0, transform.position.y, -10);
        }
    }


    void StartTimer()
    {
        oldTime = DateTime.Now;
        currentTime = DateTime.Now;
    }

    void UpdateMoveSpeed()
    {
        currentTime = DateTime.Now;
        var timePassed = currentTime - oldTime;
        if(timePassed.TotalMinutes >= 1)
        {
            oldTime = currentTime;
            cameraMoveSpeed += 0.025f;
        }
    }
}
