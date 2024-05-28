using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlowerCollectedUI : MonoBehaviour
{
    public TMP_Text flowerCollectedCyanTxt;
    public TMP_Text flowerCollectedMagentaTxt;
    public TMP_Text flowerCollectedYellowTxt;

    public void UpdateFlowersCollectedUI(int flowersCyan, int flowersMagenta, int flowersYellow)
    {
        flowerCollectedCyanTxt.text = flowersCyan.ToString();
        flowerCollectedMagentaTxt.text = flowersMagenta.ToString(); 
        flowerCollectedYellowTxt.text = flowersYellow.ToString();
    }
}
