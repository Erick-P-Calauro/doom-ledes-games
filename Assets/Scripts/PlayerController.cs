using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 10f;

    // Corpo rídigo para lógica de colisão
    private Rigidbody rb;

    // Controlador padrão para movimentos básicos
    // ATENÇÃO : Não funciona bem com rigidbody sem isKinematic : true
    private CharacterController charController;

    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -9.81f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
    }


    void OnMove(InputValue movement)
    {       
        Vector2 movementVector = movement.Get<Vector2>();

        playerX = movementVector.x;
        playerZ = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(playerX, GRAVITY, playerZ);
        charController.Move(movement * Time.deltaTime * playerSpeed);
    }   
}   
