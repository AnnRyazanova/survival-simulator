using UnityEngine;

public class RotateCube : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0,.5f,0, Space.World);
        
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
    }
}
