using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientAmount
{
    public Ingredient ingredient;
    public int amount;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public Sprite icon;
    public float timeTaken; // in seconds
    public int sellPrice;
    //note that current UI can only display max 3 ingredients
    public List<IngredientAmount> ingredients = new List<IngredientAmount>();
}
