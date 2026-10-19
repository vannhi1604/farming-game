using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectedPlant;
    public bool isPlanting = false;
    //initial money
    //if the value isn't reflected in inspector after changing this reset the script in inspector
    //(remember to reattach currency text)
    public int money = 100;
    public TMP_Text moneyTxt;

    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyTxt.text = "$" + money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectPlant(PlantItem newPlant)
    {
        if (selectedPlant == newPlant)
        {
            Debug.Log("deselected " + selectedPlant.plant.plantName);
            selectedPlant.btnImage.color = buyColor;
            selectedPlant.btnTxt.text = "Buy";
            selectedPlant = null;
            isPlanting = false;
        }
        else
        {
            if (selectedPlant != null)
            {
                selectedPlant.btnImage.color = buyColor;
                selectedPlant.btnTxt.text = "Buy";
            }
            selectedPlant = newPlant;
            selectedPlant.btnImage.color = cancelColor;
            selectedPlant.btnTxt.text = "Cancel";
            isPlanting = true;
            Debug.Log("selected " + selectedPlant.plant.plantName);
        }
    }

    public void Transaction (int value)
    {
        money += value;
        moneyTxt.text = "$" + money;
    }
}
