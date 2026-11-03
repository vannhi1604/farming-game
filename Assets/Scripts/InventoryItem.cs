using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryItem : MonoBehaviour
{
    public Ingredient item;

    public UnityEngine.UI.Image icon;
    public TMP_Text amountTxt;
    public GameObject sellButton;

    InventoryManager im;

    void Start()
    {
        im = FindAnyObjectByType<InventoryManager>();
        InitializeUI();
    }
    void InitializeUI()
    {
        icon.sprite = item.icon;
        //amountTxt.text = "x0";
    }

    //for when used as ingredients display for recipes
    public void ChangeDisplayMode()
    {
        sellButton.SetActive(false);
        GetComponent<RectTransform>().localScale = Vector3.one * 0.8f;
    }

    public void SellItem()
    {
        im.SellItem(item.itemName);
    }
}
