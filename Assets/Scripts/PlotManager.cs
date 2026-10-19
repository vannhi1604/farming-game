using UnityEngine;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    int plantStage = 0;
    float timer;

    public Color availableColor = Color.green;
    public Color unavailableColor = Color.red;
    SpriteRenderer plot;

    PlantObject selectedPlant;

    FarmManager fm;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        fm = transform.parent.GetComponent<FarmManager>();
        plot = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if theres a plant activate timer and increase plantStage until harvest
        if (isPlanted)
        {
            timer -= Time.deltaTime;

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
            //harvest is only available if plant is at its last stage and there's no plant selected
            if (plantStage == selectedPlant.plantStages.Length - 1 && !fm.isPlanting)
            {
                Harvest();
            }
        }
        //only plant if there's a plant selected already and there's enough money to buy it
        else if (fm.isPlanting && fm.selectedPlant.plant.buyPrice <= fm.money)
        {
            Plant(fm.selectedPlant.plant);
        }
    }
    private void OnMouseOver()
    {
        //if a plant is selected
        if (fm.isPlanting)
        {
            //if plot already have a plant or there isn't enough money (since money is only deducted when 
            //actually planting) give a visual indicator for the plot
            if (isPlanted || fm.selectedPlant.plant.buyPrice > fm.money)
            {
                plot.color = unavailableColor;
            }
            else
            {
                plot.color = availableColor;
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
    }
    void Plant(PlantObject newPlant)
    {
        selectedPlant = newPlant;
        isPlanted = true;
        //only deduct the money after planting
        fm.Transaction(-selectedPlant.buyPrice);
        plantStage = 0;
        UpdatePlant(); //update sprite to match
        timer = selectedPlant.timeBetweenStages;
        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        plant.sprite = selectedPlant.plantStages[plantStage];
        //update collider size according to the current sprite and recalculate the offset so it's
        //always in the middle
        plantCollider.size = plant.sprite.bounds.size;
        plantCollider.offset = new Vector2(0, plant.bounds.size.y / 2);
    }
}
