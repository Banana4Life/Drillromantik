using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class MarketItems : MonoBehaviour
{
    private MarketResource selectedItem;

    private String selectedAmount;

    private String itemPrice;

    public SliderMarketMenu slider;


    void Start()
    {
    }

    void Update()
    {
        setTextFiledsOfSelectedItem();
    }

    private void setTextFiledsOfSelectedItem()
    {
        if (selectedItem)
        {
            selectedItem.amountSelected.text = Util.FormatLargeNumber(BigInteger.One * (int) slider.sliderControl.value);
            selectedItem.currentSelectedAmount = (int) slider.sliderControl.value;
        }
    }

    public void SelectedItem(MarketResource newItem)
    {
        selectedItem = newItem;
        slider.sliderControl.minValue = Global.Resources.Items.ContainsKey(newItem.item) ? (int) -Global.Resources.Items[newItem.item] : 0;
        slider.minAmount.text = Util.FormatLargeNumber(BigInteger.One * (int) slider.sliderControl.minValue);
        
        var gold = Global.Resources.Items.ContainsKey(ItemType.GOLD) ? (int) Global.Resources.Items[ItemType.GOLD] : 0;
        foreach (var resource in gameObject.GetComponentsInChildren<MarketResource>())
        {
            if (resource == newItem)
            {
                continue;
            }
            if (resource.currentSelectedAmount<0)
            {
                gold += -resource.currentSelectedAmount* resource.goldPrice;
            }
            else
            {
                gold -= resource.currentSelectedAmount * resource.goldPrice * 2;
                
            }
        }

        slider.sliderControl.maxValue =(int) (gold / newItem.goldPrice / 2);
        slider.maxAmount.text = Util.FormatLargeNumber(BigInteger.One * (int) slider.sliderControl.maxValue);
        slider.sliderControl.value = newItem.currentSelectedAmount;
    }

    public void Trade()
    {
        var resources = new Resources();
        foreach (var resource in gameObject.GetComponentsInChildren<MarketResource>())
        {
            resources.Add(new Item() {quantity = resource.currentSelectedAmount, type = resource.item});
            var multiplier = resource.currentSelectedAmount < 0 ? -1 : -2;
            resources.Add(new Item() {quantity = resource.currentSelectedAmount * multiplier * resource.goldPrice, type = ItemType.GOLD});
            
        }
        if (Global.Resources.Add(resources))
        {
            foreach (var resource in gameObject.GetComponentsInChildren<MarketResource>())
            {
                resource.amountSelected.text = "0";
                resource.currentSelectedAmount = 0;
            }
            SelectedItem(selectedItem);
        }
    }
}