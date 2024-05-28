using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollection : MonoBehaviour
{
    public FlowerCollectedUI flowersCollectedUI;

    private int flowerCollectedCyan;
    private int flowerCollectedMagenta;
    private int flowerCollectedYellow;

    private void Start()
    {
        flowerCollectedCyan = flowerCollectedMagenta = flowerCollectedYellow = 0;
        flowersCollectedUI.UpdateFlowersCollectedUI(flowerCollectedCyan, flowerCollectedMagenta, flowerCollectedYellow);
    }

    public void OnFlowerCollected(FlowerColor flowerColor)
    {
        if (flowerColor == FlowerColor.Cyan)
            flowerCollectedCyan++;
        else if(flowerColor == FlowerColor.Magenta)
            flowerCollectedMagenta++;
        else if(flowerColor == FlowerColor.Yellow)
            flowerCollectedYellow++;

        flowersCollectedUI.UpdateFlowersCollectedUI(flowerCollectedCyan, flowerCollectedMagenta, flowerCollectedYellow);
    }
}
