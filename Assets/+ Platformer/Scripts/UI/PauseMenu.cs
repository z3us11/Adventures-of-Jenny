using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    bool openPauseMenu;

    void OnPause(InputValue inputValue)
    {
        openPauseMenu = inputValue.Get<float>() != 0 ? true : false;
        if (openPauseMenu)
        {
            if (!pauseMenu.activeSelf)
                Open();
            else
                Close();
        }
            
    }

    void Open()
    {
        Time.timeScale = 0;

        pauseMenu?.SetActive(true);
        pauseMenu?.transform.GetChild(0).DOScale(0.75f, 0);
        pauseMenu?.transform.GetChild(0).DOScale(1.1f, 0.35f);
        pauseMenu?.transform.GetChild(0).DOScale(1f, 0.15f).SetDelay(0.35f);
    }

    void Close()
    {
        Time.timeScale = 1;

        pauseMenu?.SetActive(false);
    }

    public void OnToggleSoundEffects(Toggle soundEffectsToggle)
    {
        AudioManager.instance.CanPlaySounds(soundEffectsToggle.isOn);
    }

    public void OnToggleMusic(Toggle musicToggle)
    {
        AudioManager.instance.CanPlayMusic(musicToggle.isOn);
    }
}
