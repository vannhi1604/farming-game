using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public PlantObject plant;

    public TMP_Text nameTxt;
    public TMP_Text buyPriceTxt;
    public TMP_Text sellPriceTxt;
    public TMP_Text totalTimeTxt;
    public Image icon;

    public Image btnImage;
    public TMP_Text btnTxt;

    FarmManager fm;

    void Start()
    {
        fm = FindAnyObjectByType<FarmManager>();

        InitializeUI();
    }
    public void BuyPlant()
    {
        Debug.Log("Bought " + plant.plantName);
        //automatically select the bought plant
        fm.SelectPlant(this);
    }

    void InitializeUI()
    {
        nameTxt.text = plant.plantName;
        buyPriceTxt.text = "Buy:  $" + plant.buyPrice;
        sellPriceTxt.text = "Sell: $" + plant.sellPrice;
        totalTimeTxt.text = plant.timeBetweenStages * (plant.plantStages.Length - 1) + "s";
        icon.sprite = plant.icon;
    }
}
