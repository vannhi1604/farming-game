using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private InventoryManager im;

    private void Awake()
    {
        im = FindAnyObjectByType<InventoryManager>();
    }

    public bool CanMake(Recipe recipe)
    {
        foreach (IngredientAmount ingredient in recipe.ingredients)
        {
            if (im.GetItemAmount(ingredient.ingredient)
                < ingredient.amount)
            {
                return false;
            }
        }

        return true;
    }

    public bool Make(Recipe recipe)
    {
        if (!CanMake(recipe))
            return false;

        foreach (IngredientAmount ingredient in recipe.ingredients)
        {
            im.RemoveItem(
                ingredient.ingredient,
                ingredient.amount
            );
        }

        Debug.Log("Made: " + recipe.recipeName);

        return true;
    }
}
