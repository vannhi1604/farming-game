using UnityEngine;

public class PlotManager : MonoBehaviour
{
    //plant
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    int plantStage = 0;
    float timer;
    float speed = 1f;

    //buy / select button
    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;

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
        //if theres a plant and the plot is damp activate timer and increase plantStage until harvest\
        //currently time to wait till harvest is quite short and after the initial watering it's set till
        //harvest
        //maybe kill the plant if not watered after a certain amount of time as well (just add a brownish
        //overlay over the current sprite? and give no money when harvested)
        //but that would require multiple waterings over the lifespan
        //and when the plant becomes tall enough it's really cluttered making it hard to see which plot is
        //dry
        if (isPlanted && !isDry)
        {
            timer -= speed * Time.deltaTime;

            if (timer < 0 && plantStage < selectedPlant.plantStages.Length - 1)
            {
                timer = selectedPlant.timeBetweenStages;
                plantStage++;
                UpdatePlant();
            }
        }
    }

    private void OnMouseDown() {
        if (isPlanted)
        {
            //harvesting is only done if plant is at its last stage and there's no plant / tool selected
            if (plantStage == selectedPlant.plantStages.Length - 1 && !fm.isPlanting && !fm.isSelecting)
            {
                Harvest();
            }
        }
        //only plant if plot is unlocked, there's a plant selected already and there's enough money to buy it
        else if (isBought && fm.isPlanting && fm.selectedPlant.plant.buyPrice <= fm.money)
        {
            Plant(fm.selectedPlant.plant);
        }
        if (fm.isSelecting)
        {
            switch(fm.selectedTool)
            {
                //water
                case 1:
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        if (isPlanted)
                        {
                            UpdatePlant();
                        }
                    }
                    break;
                //fertilizer
                case 2:
                    if (fm.money >= StoreManager.fertilizerPrice && isBought)
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
        //if selecting a plant
        if (fm.isPlanting)
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
        if (fm.isSelecting)
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
        //reset speed back to default
        speed = 1f;
    }
    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;
        //only deduct the money after planting
        fm.Transaction(-selectedPlant.buyPrice);
        plantStage = 0;
        UpdatePlant(); //update sprite to match
        //set up timer
        timer = selectedPlant.timeBetweenStages;

        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        if (isDry)
        {
            plant.sprite = selectedPlant.dryPlanted;
        }
        else
        {
            plant.sprite = selectedPlant.plantStages[plantStage];
        }
        //update collider size according to the current sprite and recalculate the offset so it's
        //always in the middle
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }
}
