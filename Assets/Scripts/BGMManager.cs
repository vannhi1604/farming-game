using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        instance = this;
        //so that this persists through scenes
        DontDestroyOnLoad(gameObject);
    }
}
