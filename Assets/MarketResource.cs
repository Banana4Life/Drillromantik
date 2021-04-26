using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketResource : MonoBehaviour
{
    public ItemType item;

    public Text amountSelected;

    public Text itemPrice;

    public int goldPrice;

    public int currentSelectedAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        itemPrice.text = goldPrice + " Gold";
        amountSelected.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
