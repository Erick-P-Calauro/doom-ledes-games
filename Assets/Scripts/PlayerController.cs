using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 20f;

    // ATENÇÃO : Não funciona bem com rigidbody com isKinematic = false;
    private CharacterController charController;

    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -9.81f;
    
    void Start()
    {
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
        // Precisa levar em conta up, right e forward para se mover com base na rotação da câmera
        Vector3 movement = playerX * transform.right + GRAVITY * transform.up + playerZ * transform.forward;
        charController.Move(movement * Time.deltaTime * playerSpeed);
    }   
}   
