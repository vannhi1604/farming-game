using UnityEngine;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject storage;

    CanvasGroup shopCanvasGroup;
    CanvasGroup storageCanvasGroup;

    void Awake()
    {
        shopCanvasGroup = shop.GetComponent<CanvasGroup>();
        storageCanvasGroup = storage.GetComponent<CanvasGroup>();
        //make shop the default tab
        OpenShop();
    }

    public void OpenShop()
    {
        shopCanvasGroup.alpha = 1;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;

        storageCanvasGroup.alpha = 0;
        storageCanvasGroup.interactable = false;
        storageCanvasGroup.blocksRaycasts = false;
    }

    public void OpenStorage()
    {
        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;

        storageCanvasGroup.alpha = 1;
        storageCanvasGroup.interactable = true;
        storageCanvasGroup.blocksRaycasts = true;
    }
}
