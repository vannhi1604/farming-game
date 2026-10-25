using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class PlantObject : ScriptableObject
{
    public string plantName;
    public Sprite[] plantStages;
    public float timeBetweenStages; //in seconds
    public int buyPrice;
    public int sellPrice;
    public Sprite icon;
    public Sprite dryPlanted;
}
