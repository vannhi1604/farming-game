using UnityEngine;

public class IsometricZEnvi : MonoBehaviour
{
    void Start()
    {
        //the number 100 is kinda arbitary but it will work for now...
        //automatically set z according to y to make sure each plot is displayed nicely
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 90);
    }
}
