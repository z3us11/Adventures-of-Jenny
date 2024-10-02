using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Hidden Path")]
    public Light2D globalLight;
    public Volume globalEffects;
    public Image hiddenPathTint;
    public Color outsideColor;
    public Color insideColor;
    [Space]
    public Material skyColor;
    public Color daySkyTopColor;
    public Color daySkyBottomColor;
    public Color insideSkyTopColor;
    public Color nightSkyTopColor;
    public Color nightSkyBottomColor;

    [Header("Sky")]
    public GameObject stars;
    public CanvasGroup backgroundDay;
    public CanvasGroup backgroundNight;
    public float dayTime;
    public float transitionTime;
    public float nightTime;
    public bool simulate;
    float elapsedTime;
   
    public bool isNight = false;
    public bool timeChanging = false;
    public bool isInHiddenPath = false;

    [Header("UI")]
    public TMP_Text fpsTxt;
    private float deltaTime = 0.0f;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(!isNight)
        {
            globalLight.color = outsideColor;
            skyColor.SetColor("_TopColor", daySkyTopColor);
            skyColor.SetColor("_BottomColor", daySkyBottomColor);

            AudioManager.instance.SwitchDay(false, 0);

            stars.SetActive(false);
        }
        else
        {
            globalLight.color = insideColor;
            skyColor.SetColor("_TopColor", nightSkyTopColor);
            skyColor.SetColor("_BottomColor", nightSkyBottomColor);

            AudioManager.instance.SwitchDay(true, 1);

            stars.SetActive(true);
        }
        backgroundNight.gameObject.SetActive(false);
    }

    void ChangeTime(bool _isNight, float changeDuration = 1f)
    {
        if(!_isNight)
        {
            //if(isInHiddenPath)
            //{
            //    isNight = !isNight;
            //    timeChanging = false;
            //    elapsedTime = 0;
            //    return;
            //}

            
            DOTween.To(() => globalLight.color, x => globalLight.color = x, outsideColor, changeDuration);
            skyColor.DOColor(daySkyTopColor, "_TopColor", changeDuration);
            skyColor.DOColor(daySkyBottomColor, "_BottomColor", changeDuration).OnComplete(()=> 
                { 
                    isNight = !isNight; 
                    timeChanging = false; 
                    elapsedTime = 0; 
                });

            backgroundDay.gameObject.SetActive(true);
            backgroundDay.DOFade(0, 0);
            backgroundDay.DOFade(0.96f, 1f).OnComplete(()=> 
                {
                    backgroundDay.DOFade(1, changeDuration/2).OnComplete(() => { stars.SetActive(false); backgroundDay.gameObject.SetActive(false); });
                });
        }
        else
        {
            DOTween.To(() => globalLight.color, x => globalLight.color = x, insideColor, changeDuration);
            skyColor.DOColor(nightSkyTopColor, "_TopColor", changeDuration);
            skyColor.DOColor(nightSkyBottomColor, "_BottomColor", changeDuration).OnComplete(() =>
                {
                    isNight = !isNight;
                    timeChanging = false;
                    elapsedTime = 0;
                    stars.SetActive(true);
                    backgroundNight.gameObject.SetActive(true);
                    backgroundNight.DOFade(1, 0);
                    backgroundNight.DOFade(0.96f, changeDuration - 1f).OnComplete(() =>
                    {
                        backgroundNight.DOFade(0, 1f).OnComplete(() => { backgroundNight.gameObject.SetActive(false); timeChanging = false; elapsedTime = 0; });
                    });
                });

            
        }

        AudioManager.instance.SwitchDay(_isNight, changeDuration);

    }

    private void Update()
    {
        //if (!timeChanged)
        //{
        //    timeChanged = true;
        //    ChangeTime(isNight);
        //}

        if (simulate)
            DayCycle();

        // Calculate frame time
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculate FPS
        float fps = 1.0f / deltaTime;

        // Display FPS in the UI Text
        fpsTxt.text = string.Format("{0:0.} FPS", fps);

    }

    private void DayCycle()
    {
        if (!isNight)
        {
            if(elapsedTime < dayTime)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                if (timeChanging)
                    return;

                timeChanging = true;
                ChangeTime(!isNight, transitionTime);
            }
        }
        else
        {
            if (elapsedTime < nightTime)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                if (timeChanging)
                    return;

                timeChanging = true;
                ChangeTime(!isNight, transitionTime);
            }
        }
    }

    void SwitchTime(bool _isNight)
    {

    }
}
