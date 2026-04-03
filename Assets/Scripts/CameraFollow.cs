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
        Debug.Log("billboard rodando");
        transform.rotation = cam.transform.rotation;
    }
}
