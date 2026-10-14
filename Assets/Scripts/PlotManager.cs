using UnityEngine;

public class PlotManager : MonoBehaviour
{
    bool isPlanted = false;
    SpriteRenderer plant;
    BoxCollider2D plantCollider;
    int plantStage = 0;
    float timer;

    public PlantObject selectedPlant;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plant = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plantCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
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
            //only harvest if plant is at its last stage
            if (plantStage == selectedPlant.plantStages.Length - 1)
            {
                Harvest();
            }
        }
        else
        {
            Plant();
        }
    }
    void Harvest()
    {
        isPlanted = false;
        plant.gameObject.SetActive(false);
    }
    void Plant()
    {
        isPlanted = true;
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
