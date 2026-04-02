using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// ATENÇÃO : Esse script pressupõe que o container de player é pai do container com este script.
public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.25f;
    private float rotationCamera;
    private float rotationPlayer;
    
    private GameObject playerHands;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Inicializa rotationPlayer com valores de rotação do player
        rotationPlayer = transform.parent.transform.eulerAngles.y;

        playerHands = GameObject.FindGameObjectWithTag("CameraAttach");
    }

    void Update()
    {
        RefreshRotationValues(Mouse.current.delta.ReadValue());     
        RotateCameraAndPlayer();
    }
    
    // Evita stuttering na movimentação da mão.
    void LateUpdate()
    {
        playerHands.transform.rotation = Quaternion.Euler(rotationCamera, rotationPlayer, 0f);
    }

    void RefreshRotationValues(Vector2 mousePosition)
    {
        rotationCamera -= sensitivity * mousePosition.y;
        rotationPlayer += sensitivity * mousePosition.x;

        rotationCamera = math.clamp(rotationCamera, -90, 90);
    }

    void RotateCameraAndPlayer()
    {
        transform.localRotation = Quaternion.Euler(rotationCamera, 0, 0);
        transform.parent.transform.rotation = Quaternion.Euler(0, rotationPlayer, 0);
    }
}
