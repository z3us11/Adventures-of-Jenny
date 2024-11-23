using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public TMP_Text pressStartTxt;
    public Image fadeOut;

    private void Start()
    {
        pressStartTxt.DOFade(0, 1f).OnComplete(() => pressStartTxt.DOFade(1, 1f)).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
           SwitchScene();
            
        }
    }

    public void SwitchScene()
    {
        fadeOut.DOFade(1, 1f).OnComplete(() => SceneManager.LoadScene("Platformer"));
    }
}
