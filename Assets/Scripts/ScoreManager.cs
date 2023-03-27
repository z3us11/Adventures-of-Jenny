using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTxt;

    int score = 0;
    public static ScoreManager instance;

    private void Awake()
    {
        instance = this;
        scoreTxt.text = "Score : 0";
    }

    public void UpdateScore(int value)
    {
        if(value > score)
        {
            score = value;
            scoreTxt.text = "Score : " + score.ToString();
        }
    }
}
