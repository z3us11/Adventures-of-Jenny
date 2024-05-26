using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreTxt;
    [SerializeField] TMP_Text bestScoreTxt;
    [SerializeField] TMP_Text lastScoreTxt;
    [SerializeField] TMP_Text comboTxt;

    int score = 0;
    int combo = 0;
    public static ScoreManager instance;

    Tween tween;
    private void Awake()
    {
        instance = this;
        scoreTxt.text = "Score : 0";
        comboTxt.text = "Combo : 0";
        bestScoreTxt.text = "Best Score : " + PlayerPrefs.GetInt("BestScore", 0);
        lastScoreTxt.text = "Last Score : " + PlayerPrefs.GetInt("LastScore", 0);
    }

    public void UpdateFinalScores()
    {
        PlayerPrefs.SetInt("LastScore", score);
        lastScoreTxt.text = "Last Score : " + score.ToString();

        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreTxt.text = "Best Score : " + score.ToString();
        }
    }

    public void UpdateScore(int value)
    {
        if(value > score)
        {
            score = value;
            scoreTxt.text = "Score : " + score.ToString();
        }
    }

    public void ComboRegistering(int comboScore)
    {
        if (comboScore == -1)
            combo = 0;
        else
        {
            combo += comboScore;
            comboTxt.text = "Combo : " + combo.ToString();
            comboTxt.GetComponent<CanvasGroup>().alpha = 1;

            tween.Kill();
            comboTxt.transform.DOPunchScale(Vector3.one * 0.25f, 0.5f).SetEase(Ease.InSine);
            tween = comboTxt.GetComponent<CanvasGroup>().DOFade(0, 2f);
            //tween = comboTxt.DOColor(new Color(255, 255, 255, 0), 2f); 
        }
        //Debug.Log(combo + " | " + comboScore);
    }
}
