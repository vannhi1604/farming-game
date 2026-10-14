using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public PlantItem selectedPlant;
    public bool isPlanting = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            selectedPlant = null;
            isPlanting = false;
        }
        else
        {
            selectedPlant = newPlant;
            isPlanting = true;
            Debug.Log("selected " + selectedPlant.plant.plantName);
        }
    }
}
