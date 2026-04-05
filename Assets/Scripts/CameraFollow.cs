using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
    }
}
