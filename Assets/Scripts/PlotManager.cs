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
    int pestStage;
    int weedStage;
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
    bool hasPest = false;
    bool hasWeed = false;
    public bool isBought = true;
    public Sprite drySprite;
    public Sprite normalSprite;
    public Sprite unavailableSprite;

    [SerializeField] private GameObject pest;
    [SerializeField] private GameObject weed;

    PlantObject selectedPlant;

    FarmManager fm;

    InventoryManager im;

    //audio
    AudioSource audioSource;
    [SerializeField] private AudioClip plantSound;
    [SerializeField] private AudioClip harvestSound;
    [SerializeField] private AudioClip wateringSound;
    [SerializeField] private AudioClip herbipestiSound;
    [SerializeField] private AudioClip unlockPlotSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        sm = GetComponent<StatusManager>();
        im = FindAnyObjectByType<InventoryManager>();
        plot = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (!isBought)
        {
            plot.sprite = unavailableSprite;
        }
        else
        {
            plot.sprite = drySprite;
        }
        weed.SetActive(false);
        pest.SetActive(false);
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
                if (isDry || hasPest || hasWeed)
                {
                    if (plantStage != 0)
                    {
                        KillPlant();
                    }
                    //else if not watered first stage plant just doesn't grow (pests and weeds won't appear
                    //in first stage)
                }
                else
                {
                    timer = selectedPlant.timeBetweenStages;
                    plantStage++;
                    UpdatePlant();
                    //dry out the plot / add pest / add weed when the designated stage is reached
                    if (plantStage == dryStage)
                    {
                        isDry = true;
                        plot.sprite = drySprite;
                        sm.ChangeWaterIconDisplay(true);
                    }
                    if (plantStage == pestStage)
                    {
                        hasPest = true;
                        pest.SetActive(true);
                        sm.ChangePestIconDisplay(true);
                    }
                    if (plantStage == weedStage)
                    {
                        hasWeed = true;
                        weed.SetActive(true);
                        sm.ChangeWeedIconDisplay(true);
                    }
                }
            }
        }
    }

    private void OnMouseDown() {
        //harvesting / removing dead plants will override tools (tools get deselected, effects not appiled,
        //no money deducted if tool costs money since there's no effect applied)
        if (isPlanted)
        {
            //harvesting is only done if plant is at its last stage, this will also deselect any tool / plant
            //currently selected
            //plants selected will not go over this like when removing dead plants
            if (plantStage == selectedPlant.plantStages.Length - 1)
            {
                fm.ResetSelection();
                Harvest();
                return;
            }
            //if the plant is dead, reset plot and deselect all tools
            //or plant if there's a plant selected and enough money
            else if (hasDeadPlant)
            {
                ResetPlot();

                if (fm.isPlanting && fm.selectedPlant.plant.buyPrice <= fm.money)
                {
                    Plant(fm.selectedPlant.plant);
                }
                else
                {
                    fm.ResetSelection();

                    //play sound effect
                    audioSource.volume = 1f;
                    audioSource.PlayOneShot(harvestSound);
                }

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
                    if (isBought)
                    {
                        isDry = false;
                        plot.sprite = normalSprite;
                        sm.ChangeWaterIconDisplay(false);

                        //play sound effect
                        audioSource.volume = 0.1f;
                        audioSource.PlayOneShot(wateringSound);
                    }
                    break;
                //herbicide
                //does nothing if plot has no weed to prevent accidentally spending money
                case 2:
                    if (isBought && isPlanted && hasWeed && fm.money >= StoreManager.herbicidePrice)
                    {
                        fm.Transaction(-StoreManager.herbicidePrice);
                        hasWeed = false;

                        weed.SetActive(false);
                        sm.ChangeWeedIconDisplay(false);

                        //play sound effect
                        audioSource.volume = 0.4f;
                        audioSource.PlayOneShot(herbipestiSound);
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

                        //play sound effect
                        audioSource.volume = 0.1f;
                        audioSource.PlayOneShot(unlockPlotSound);
                    }
                    break;
                //pesticide
                //does nothing if plot has no weed to prevent accidentally spending money
                case 4:
                    if (isBought && isPlanted && hasPest && fm.money >= StoreManager.pesticidePrice)
                    {
                        fm.Transaction(-StoreManager.pesticidePrice);
                        hasPest = false;

                        pest.SetActive(false);
                        sm.ChangePestIconDisplay(false);

                        //play sound effect
                        audioSource.volume = 0.4f;
                        audioSource.PlayOneShot(herbipestiSound);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    private void OnMouseOver()
    {
        if (isPlanted)
        {
            if (plantStage == selectedPlant.plantStages.Length - 1)
            {
                plot.color = harvestColor;
                return;
            }
            else if (hasDeadPlant)
            {
                plot.color = availableColor;
                return;
            }
        }
        //if selecting a plant
        if (fm.isPlanting)
        {
            //if plot is locked / already have a plant that isn't dead / there isn't enough money (since
            //money is only deducted when actually planting) give a visual indicator
            if (!isBought || (isPlanted && !hasDeadPlant) || fm.selectedPlant.plant.buyPrice > fm.money)
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
                //herbicide (can't be used on plots with no weeds)
                case 2:
                    if (isBought && isPlanted && hasWeed && fm.money >= StoreManager.herbicidePrice)
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
                //pesticide (can't be used on plots with no pests)
                case 4:
                    if (isBought && isPlanted && hasPest && fm.money >= StoreManager.pesticidePrice)
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
    void ResetPlot()
    {
        isPlanted = false;
        hasDeadPlant = false;
        plant.color = Color.white;
        plant.gameObject.SetActive(false);
        //set plot back to dry
        isDry = true;
        plot.sprite = drySprite;
        //remove pests and weeds
        hasPest = false;
        pest.SetActive(false);
        hasWeed = false;
        weed.SetActive(false);
        //remove all statuses
        sm.ChangeWaterIconDisplay(false);
        sm.ChangePestIconDisplay(false);
        sm.ChangeWeedIconDisplay(false);
        //reset stats back to default
        dryStage = -1;
        pestStage = -1;
        weedStage = -1;
    }
    void Harvest()
    {
        ResetPlot();
        //switch to adding to inventory instead of immediately selling it
        im.AddItem(selectedPlant.plantName, 1);
        //fm.Transaction(selectedPlant.sellPrice);
        //play sound effect
        audioSource.volume = 1f;
        audioSource.PlayOneShot(harvestSound);
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
        //roll between will and won't have pest / weed, if will have, do another roll for the stage
        //rate is currently 20%
        pestStage = Random.Range(-3, 2);
        if (pestStage == 1)
        {
            pestStage = Random.Range(1, selectedPlant.plantStages.Length - 1);
        }
        weedStage = Random.Range(-3, 2);
        if (weedStage == 1)
        {
            weedStage = Random.Range(1, selectedPlant.plantStages.Length - 1);
        }
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
        //play sound effect
        audioSource.volume = 1f;
        audioSource.PlayOneShot(plantSound);
    }

    void UpdatePlant()
    {
        plant.sprite = selectedPlant.plantStages[plantStage];
        //update plant's collider size according to the current sprite and recalculate the offset so it's
        //always in the middle
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }

    void KillPlant()
    {
        plant.color = deadPlantColor;
        hasDeadPlant = true;
    }
}
