using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderMarketMenu : MonoBehaviour
{
    public Text currentAmount;

    public Text maxAmount;

    public Text minAmount;

    public Slider sliderControl;

    void Start()
    {
        sliderControl = GetComponent<Slider>();
        SetMinAmount();
        SetMaxAmount();
        AdjustCurrentAmount();
    }

    private void SetMinAmount(int newMinAmount = 0)
    {
        sliderControl.minValue = newMinAmount;
        minAmount.text = newMinAmount.ToString();
    }
    private void SetMaxAmount(int newMaxAmount = 0)
    {
        sliderControl.maxValue = newMaxAmount;
        maxAmount.text = newMaxAmount.ToString();
    }

    public void AdjustCurrentAmount()
    {
        currentAmount.text = sliderControl.value.ToString();
    }

    public void IncrementAmountByOne()
    {
        sliderControl.value += 1;
    }
    
    public void DecrementAmountByOne()
    {
        sliderControl.value -= 1;
    }
}