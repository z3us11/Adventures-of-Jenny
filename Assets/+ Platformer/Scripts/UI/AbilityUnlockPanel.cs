using Platformer;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AbilityUnlockPanel : MonoBehaviour
{
    public GameObject mainPanel;
    public TMP_Text abilityNameTxt;
    public TMP_Text abilityDescriptionTxt;
    public Button okButton;

    [SerializeField] PlayerController playerController;

    private void Start()
    {
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(Close);
    }

    public void SetupAbilityUnlockPanel(AbilitySO ability)
    {
        Open();
        abilityNameTxt.text = ability.abilityName;
        abilityDescriptionTxt.text = ability.abilityDescription;
        playerController.canPlayerMove = false;
    }

    void Open()
    {
        gameObject.SetActive(true);
        mainPanel.transform.DOScale(0, 0);
        mainPanel.transform.DOScale(1, 0.5f);
    }

    void Close()
    {
        playerController.canPlayerMove = true;
        mainPanel.transform.DOScale(1, 0);
        mainPanel.transform.DOScale(0, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
