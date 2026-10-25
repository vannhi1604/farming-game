using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public GameObject plantItem;
    List<PlantObject> plantObjects = new List<PlantObject>();

    public const int fertilizerPrice = 10;
    private void Awake()
    {
        var loadedPlants = Resources.LoadAll("Plants", typeof(PlantObject));
        foreach(var plant in loadedPlants)
        {
            plantObjects.Add((PlantObject)plant);
        }
        plantObjects.Sort(SortByPrice);
        foreach (var plant in plantObjects)
        {
            PlantItem newPlant = Instantiate(plantItem, transform).GetComponent<PlantItem>();
            newPlant.plant = plant;
        }
    }

    int SortByPrice(PlantObject a, PlantObject b)
    {
        return a.buyPrice.CompareTo(b.buyPrice);
    }
}
