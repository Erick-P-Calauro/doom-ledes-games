using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;
   
    void Start()
    {
        cam = Camera.main;
        
    }
    
    void Update()
    {
        transform.rotation = cam.transform.rotation;
    }
}