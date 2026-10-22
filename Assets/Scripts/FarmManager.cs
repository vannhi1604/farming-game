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

    public bool isSelecting = false;
    //1 = water, 2 = fertilizer, 3 = new plot
    public int selectedTool = 0;

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;
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
            ResetSelection();
        }
        else
        {
            ResetSelection();
            selectedPlant = newPlant;
            selectedPlant.btnImage.color = cancelColor;
            selectedPlant.btnTxt.text = "Cancel";
            isPlanting = true;
            Debug.Log("selected " + selectedPlant.plant.plantName);
        }
    }

    public void SelectTool(int toolNumber)
    {
        if (selectedTool == toolNumber)
        {
            ResetSelection();
        }
        else
        {
            ResetSelection();
            isSelecting = true;
            selectedTool = toolNumber;
            buttonsImg[toolNumber - 1].sprite = selectedButton;
        }
    }

    //reset all selections to make sure the player can only either plant or use a tool at a time since this
    //will be called for every selection
    void ResetSelection()
    {
        if (isPlanting)
        {
            isPlanting = false;
            //reset buy button to default state
            if (selectedPlant != null)
            {
                selectedPlant.btnImage.color = buyColor;
                selectedPlant.btnTxt.text = "Buy";
                selectedPlant = null;
            }
        }
        if (isSelecting)
        {
            //reset tool button to default state
            if (selectedTool > 0)
            {
                buttonsImg[selectedTool - 1].sprite = normalButton;
            }
            isSelecting = false;
            selectedTool = 0;
        }
    }

    public void Transaction (int value)
    {
        money += value;
        moneyTxt.text = "$" + money;
    }
}
