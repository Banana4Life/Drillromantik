using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MarketMenu : MonoBehaviour
{

    private Text amountText;

    public Slider sliderControl;
    
    void Start()
    {
        amountText = GetComponent<Text>();
    }

    public void adjustAmount()
    {
        amountText.text = sliderControl.value.ToString();
    }
}
