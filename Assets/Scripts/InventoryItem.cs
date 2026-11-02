using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Ingredient item;

    public Image icon;
    public TMP_Text amountTxt;

    void Start()
    {
        InitializeUI();
    }
    void InitializeUI()
    {
        icon.sprite = item.icon;
        amountTxt.text = "x0";
    }
}
