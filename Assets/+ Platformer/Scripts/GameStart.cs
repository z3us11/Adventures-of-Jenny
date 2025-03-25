using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Platformer
{
    public class GameStart : MonoBehaviour
    {
        public TMP_Text pressStartTxt;
        public Image fadeOut;

        bool startPressed;

        private void Start()
        {
            pressStartTxt.DOFade(0, 1f).OnComplete(() => pressStartTxt.DOFade(1, 1f)).SetLoops(-1, LoopType.Yoyo);
        }

        void OnStart(InputValue inputValue)
        {
            startPressed = inputValue.Get<float>() != 0 ? true : false;
        }

        private void Update()
        {
            if (startPressed)
            {
                SwitchScene();
                startPressed = false;
                return;
            }
        }

        public void SwitchScene()
        {
            fadeOut.DOFade(1, 1f).OnComplete(() => SceneManager.LoadScene("Platformer"));
        }
    }

}

