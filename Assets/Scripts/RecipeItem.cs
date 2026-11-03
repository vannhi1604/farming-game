using System.Collections.Generic;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RecipeItem : MonoBehaviour
{
    public Recipe recipe;
    float timer;
    bool making = false;
    bool claimable = false;

    //for displaying ingredients in UI
    public GameObject inventoryItem;
    public Transform ingredientsContainer;

    //UI elements
    public Image icon;
    public TMP_Text nameTxt;
    public TMP_Text timeTakenTxt;
    public TMP_Text sellPriceTxt;

    public Image btnImage;
    public TMP_Text btnTxt;

    InventoryManager im;

    FarmManager fm;

    List<InventoryItem> itemsUI = new List<InventoryItem>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        im = FindAnyObjectByType<InventoryManager>();
        fm = FindAnyObjectByType<FarmManager>();

        InitializeUI();
    }

    private void Update()
    {
        if (making)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay();
            if (timer < 0)
            {
                making = false;
                claimable = true;

                btnImage.color = Color.green;
                btnTxt.text = "Claim";
            }
        }
        else if (claimable == false && CanMake())
        {
            btnImage.color = Color.green;
        }
        else if (claimable == false && !CanMake())
        {
            btnImage.color = Color.gray;
        }
    }

    public void OnButtonClick()
    {
        //if idleing, make the item
        if (making == false && claimable == false)
        {
            Make();
        }
        //if ready to claim, claim it and sell it
        else if (making == false && claimable == true)
        {
            claimable = false;

            fm.Transaction(recipe.sellPrice);

            btnImage.color = Color.green;
            btnTxt.text = "Make";
        }
    }

    void InitializeUI()
    {
        icon.sprite = recipe.icon;
        nameTxt.text = recipe.recipeName;
        timeTakenTxt.text = recipe.timeTaken + "s";
        sellPriceTxt.text = "Sell: $" + recipe.sellPrice;

        foreach (var ingredientListItem in recipe.ingredients)
        {
            InventoryItem ingredientUI = Instantiate(inventoryItem, ingredientsContainer)
                .GetComponent<InventoryItem>();
            ingredientUI.item = ingredientListItem.ingredient;
            ingredientUI.amountTxt.text = "x" + ingredientListItem.amount;
            ingredientUI.ChangeDisplayMode();
        }
    }
    
    bool CanMake()
    {
        foreach (IngredientAmount ingredient in recipe.ingredients)
        {
            if (im.GetItemAmount(ingredient.ingredient) < ingredient.amount)
            {
                return false;
            }
        }

        return true;
    }

    void Make()
    {
        if (!CanMake())
        {
            return;
        }

        foreach (IngredientAmount ingredient in recipe.ingredients)
        {
            im.RemoveItem(ingredient.ingredient, ingredient.amount);
        }

        timer = recipe.timeTaken;
        making = true;
        claimable = false;

        btnImage.color = Color.yellow;
        //display timer here

        Debug.Log("Making: " + recipe.recipeName);

        return;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        btnTxt.text = string.Format("{0:00}:{1:00}",
            minutes, seconds);
    }
}
