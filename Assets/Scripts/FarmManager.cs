using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectedPlant;
    public bool isPlanting = false;
    //initial money
    //if the value isn't reflected in inspector after changing this change it in the inspector or
    //reset the script in inspector (remember to reattach components)
    public int money = 100;
    public TMP_Text moneyTxt;
    //plot prcing
    int basePlotPrice = 150;
    public int currentPlotPrice = 150;
    const double growthRate = 0.35;
    int step = 1;
    public TMP_Text plotPriceTxt;
    //for shop buttons
    public Color buyColor = Color.green;
    public Color cancelColor = Color.red;

    public bool isSelecting = false;
    //1 = water, 2 = fertilizer, 3 = new plot
    public int selectedTool = 0;

    public Image[] buttonsImg;
    public Sprite normalButton;
    public Sprite selectedButton;

    //audio
    AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        moneyTxt.text = "$" + money;
        plotPriceTxt.text = "$" + currentPlotPrice;
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

            //play sound effect
            audioSource.PlayOneShot(clickSound);
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

            //play sound effect
            audioSource.PlayOneShot(clickSound);
        }
    }

    //reset all selections to make sure the player can only either plant or use a tool at a time since this
    //will be called for every selection
    public void ResetSelection()
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

    //0.30: 150, 195, 255, 330, 430, 560, 725, 945, 1225, 1595, 2070, 2690, 3495
    //(current) 0.35: 150, 205, 275, 370, 500, 675, 910, 1230, 1655, 2235, 3020, 4075, 5500
    public void increasePlotPrice()
    {
        currentPlotPrice = (int)(System.Math.Ceiling(basePlotPrice * System.Math.Pow(1 + growthRate, step) / 5) * 5);
        step++;
        plotPriceTxt.text = "$" + currentPlotPrice;
    }
}
