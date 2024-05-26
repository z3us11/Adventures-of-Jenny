using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("regen"))
        {
            Debug.Log("Game Over! Restarting");
            ScoreManager.instance.UpdateFinalScores();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
