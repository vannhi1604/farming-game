using UnityEngine;

public class PlotManager : MonoBehaviour
{
    //plant
    bool isPlanted = false;
    bool hasDeadPlant = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    int plantStage = 0;
    //stage where the plant dries out, will be set when planting
    int dryStage;
    float timer;
    float speed = 1f;

    //visual indicator
    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;
    public Color harvestColor = Color.gold;
    public Color deadPlantColor = Color.brown; //this is a bit too dark
    StatusManager sm;

    //plot
    SpriteRenderer plot;
    bool isDry = true;
    public bool isBought = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    PlantObject selectedPlant;

    FarmManager fm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        sm = GetComponent<StatusManager>();
        plot = GetComponent<SpriteRenderer>();
        if (!isBought)
        {
            plot.sprite = unavailableSprite;
        }
        else
        {
            plot.sprite = drySprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if there's a plant and it can still grow activate timer and increase plantStage
        if (isPlanted && !hasDeadPlant)
        {
            timer -= speed * Time.deltaTime;

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                //plant dies if not watered before it reaches the next stage (and if not at the first stage
                //so that multiple plants can be planted then watered at once without the time constraint 
                //caused by this)
                if (isDry)
                {
                    if (plantStage != 0)
                    {
                        plant.color = deadPlantColor;
                        hasDeadPlant = true;
                    }
                    //else if at first stage plant just doesn't grow
                }
                else
                {
                    timer = selectedPlant.timeBetweenStages;
                    plantStage++;
                    UpdatePlant();
                    //dry out the plot when the designated stage is reached
                    if (plantStage == dryStage)
                    {
                        isDry = true;
                        plot.sprite = drySprite;
                        sm.ChangeWaterIconDisplay(true);
                    }
                }
            }
        }
    }

    private void OnMouseDown() {
        if (isPlanted)
        {
            //harvesting is only done if plant is at its last stage, this will also deselect any tool / plant
            //currently selected
            if (plantStage == selectedPlant.plantStages.Length - 1)
            {
                fm.ResetSelection();
                Harvest();
                return;
            }
        }
        //only plant if plot is unlocked, there's a plant selected already and there's enough money to buy it
        else if (fm.isPlanting && isBought && fm.selectedPlant.plant.buyPrice <= fm.money)
        {
            Plant(fm.selectedPlant.plant);
        }
        if (fm.isSelecting)
        {
            switch(fm.selectedTool)
            {
                //water
                case 1:
                    if (isBought && !hasDeadPlant)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        sm.ChangeWaterIconDisplay(false);
                        //change plot sprite instead of updating the plant
                        //if (isPlanted)
                        //{

                        //    UpdatePlant();
                        //}
                    }
                    break;
                //fertilizer
                case 2:
                    if (fm.money >= StoreManager.fertilizerPrice && isBought && !hasDeadPlant)
                    {
                        fm.Transaction(-StoreManager.fertilizerPrice);
                        if (speed < 2) speed += .2f;
                    }
                    break;
                //unlock new plot
                case 3:
                    if (fm.money >= fm.currentPlotPrice && !isBought)
                    {
                        fm.Transaction(-fm.currentPlotPrice);
                        fm.increasePlotPrice();
                        isBought = true;
                        plot.sprite = drySprite;
                        isDry = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    private void OnMouseOver()
    {
        if (isPlanted && plantStage == selectedPlant.plantStages.Length - 1)
        {
            plot.color = harvestColor;
        }
        //if selecting a plant
        else if (fm.isPlanting)
        {
            //if plot is locked / already have a plant / there isn't enough money (since money is only deducted when 
            //actually planting) give a visual indicator
            if (!isBought || isPlanted || fm.selectedPlant.plant.buyPrice > fm.money)
            {
                plot.color = unavailableColor;
            }
            else
            {
                plot.color = availableColor;
            }
        }
        //if selecting a tool
        else if (fm.isSelecting)
        {
            switch(fm.selectedTool)
            {
                //water
                case 1:
                    if (isBought)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                //fertilizer
                case 2:
                    if (isBought && fm.money >= StoreManager.fertilizerPrice)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                //buying plots
                case 3:
                    if (!isBought && fm.money >= fm.currentPlotPrice)
                    {
                        plot.color = availableColor;
                    }
                    else
                    {
                        plot.color = unavailableColor;
                    }
                    break;
                default:
                    plot.color = unavailableColor;
                    break;
            }
        }
    }
    private void OnMouseExit()
    {
        //reset plot color when leaving
        plot.color = Color.white;
    }
    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
        fm.Transaction(selectedPlant.sellPrice);
        //change the plot back to its dry state
        isDry = true;
        plot.sprite = drySprite;
        //reset stats back to default
        speed = 1f;
        dryStage = -1;
    }
    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;
        hasDeadPlant = false;
        //only deduct the money after planting
        fm.Transaction(-selectedPlant.buyPrice);
        plantStage = 0;
        //pick any stage excluding the first and last
        dryStage = Random.Range(1, selectedPlant.plantStages.Length - 1);
        //update sprite to match
        UpdatePlant();
        //if the empty plot wasn't watered before planting, display status icon (icon is only displyed when
        //there's a plant
        if (isDry)
        {
            sm.ChangeWaterIconDisplay(true);
        }
        //set up timer
        timer = selectedPlant.timeBetweenStages;
        //make the plant visible
        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        plant.sprite = selectedPlant.plantStages[plantStage];
        //update plant's collider size according to the current sprite and recalculate the offset so it's
        //always in the middle
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }
}
