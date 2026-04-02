using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 0.25f;
    public GameObject player;

    private float rotationCamera;
    private float rotationPlayer;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotationPlayer = player.transform.eulerAngles.y;
    }

    void Update()
    {
        Vector2 mousePos = sensitivity * Mouse.current.delta.ReadValue();
        
        rotationCamera -= mousePos.y;
        rotationPlayer += mousePos.x;

        rotationCamera = math.clamp(rotationCamera, -90, 90);
    
        transform.localRotation = Quaternion.Euler(rotationCamera, 0, 0);
        player.transform.rotation = Quaternion.Euler(0, rotationPlayer, 0);
    }
}
