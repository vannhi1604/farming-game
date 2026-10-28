using UnityEngine;
using UnityEngine.TextCore.Text;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private GameObject waterIcon;
    [SerializeField] private GameObject pestIcon;
    [SerializeField] private GameObject weedIcon;

    [SerializeField] private float yOffset = 0.5f;

    SpriteRenderer plant;

    void Start()
    {
        plant = transform.Find("Plant").GetComponent<SpriteRenderer>();

        //setting this through inspector cause all icons to be disabled throughout the game even though the
        //function runs as normal?
        ChangeWaterIconDisplay(false);
        ChangePestIconDisplay(false);
        ChangeWeedIconDisplay(false);
    }
    public void ChangeWaterIconDisplay(bool status)
    {
        waterIcon.SetActive(status);
        AdjustPosition(waterIcon);
    }

    public void ChangePestIconDisplay(bool status)
    {
        pestIcon.SetActive(status);
        AdjustPosition(pestIcon);
    }

    public void ChangeWeedIconDisplay(bool status)
    {
        weedIcon.SetActive(status);
        AdjustPosition(weedIcon);
    }

    void AdjustPosition(GameObject icon)
    {
        icon.transform.position = new Vector3(
            icon.transform.position.x,
            plant.bounds.max.y + yOffset,
            icon.transform.position.z
        );
    }
}
