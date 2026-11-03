using System;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public GameObject recipeItem;

    //objects loaded
    Recipe[] loadedRecipes;

    private void Awake()
    {
        loadedRecipes = Resources.LoadAll<Recipe>("Recipes");
        Array.Sort(loadedRecipes, SortByPrice);
        foreach (var recipe in loadedRecipes)
        {
            RecipeItem newItem = Instantiate(recipeItem, transform).GetComponent<RecipeItem>();
            newItem.recipe = recipe;
        }
    }
    int SortByPrice(Recipe a, Recipe b)
    {
        return a.sellPrice.CompareTo(b.sellPrice);
    }
}
