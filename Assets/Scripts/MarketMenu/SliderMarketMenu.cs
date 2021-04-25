using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderMarketMenu : MonoBehaviour
{
    public Text currentAmount;

    public Text maxAmount;

    public Text minAmount;

    private Slider _sliderControl;

    void Start()
    {
        _sliderControl = GetComponent<Slider>();
        SetMinAmount();
        SetMaxAmount();
        AdjustCurrentAmount();
    }

    private void SetMinAmount(int newMinAmount = 0)
    {
        _sliderControl.minValue = newMinAmount;
        minAmount.text = newMinAmount.ToString();
    }
    private void SetMaxAmount(int newMaxAmount = 100)
    {
        _sliderControl.maxValue = newMaxAmount;
        maxAmount.text = newMaxAmount.ToString();
    }

    public void AdjustCurrentAmount()
    {
        currentAmount.text = _sliderControl.value.ToString(CultureInfo.InvariantCulture);
    }

    public void IncrementAmountByOne()
    {
        _sliderControl.value += 1;
    }
    
    public void DecrementAmountByOne()
    {
        _sliderControl.value -= 1;
    }
}