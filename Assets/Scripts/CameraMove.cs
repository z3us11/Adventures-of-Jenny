using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] float playerPosBeforeMove;
    [SerializeField] float cameraMoveSpeed;
    private void Update()
    {
        if(player.position.y > playerPosBeforeMove)
        {
            transform.Translate(Vector2.up * cameraMoveSpeed * Time.deltaTime);
        }
    }
}
