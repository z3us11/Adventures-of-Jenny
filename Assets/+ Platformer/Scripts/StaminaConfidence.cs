using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaConfidence : MonoBehaviour
{
    public Image staminaConfidenceMeter;
    
    private float maxStaminaConfidence = 100;
    private float currentStaminaConfidence = 50;

    private void Start()
    {
        UpdateStaminaConfidence(currentStaminaConfidence);
    }

    public float GetStaminConfidenceValue()
    {
        return currentStaminaConfidence;
    }

    public void UpdateStaminaConfidence(float amount)
    {
        currentStaminaConfidence += amount;
        if(currentStaminaConfidence < 0)
            currentStaminaConfidence = 0;
        if(currentStaminaConfidence > maxStaminaConfidence)
            currentStaminaConfidence = maxStaminaConfidence;

        staminaConfidenceMeter.fillAmount = currentStaminaConfidence / maxStaminaConfidence;
    }


}
