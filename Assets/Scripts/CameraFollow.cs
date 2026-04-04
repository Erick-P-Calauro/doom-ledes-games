using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera cam;
<<<<<<< HEAD
    // Start is called once before the first execution of Update after the MonoBehaviour is created
=======
   
>>>>>>> origin/master
    void Start()
    {
        cam = Camera.main;
        
    }
<<<<<<< HEAD

    // Update is called once per frame
    void Update()
    {
        Debug.Log("billboard rodando");
=======
    
    void Update()
    {
>>>>>>> origin/master
        transform.rotation = cam.transform.rotation;
    }
}
