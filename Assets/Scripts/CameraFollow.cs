using UnityEngine;


//Atenção, para o script funcionar a camera de visão do player deve ter a tag MainCamera
public class CameraFollow : MonoBehaviour
{
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        
    }

    void Update()
    {
        Debug.Log("billboard rodando");
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
    }
}