using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    // Campos marcados com SerializeField podem ser observados pelo Inspector
    [SerializeField] private float playerNormalSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float crouchRunningSpeed = 3f;
    [SerializeField] private float playerSpeed;

    // Campos sem SerializeField são estado interno do Player;
    private CharacterController charController;
    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -9.81f;
    private bool isCrouching = false;
    private bool isRunning = false;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerSpeed = playerNormalSpeed;
        charController.height = playerHeight;
    }

    void Update()
    {   
        playerSpeed = GetPlayerSpeed();
        
        // Precisa levar em conta up, right e forward (eixos no espaço) para se mover com base na rotação da câmera
        Vector3 movementVector = (playerX * transform.right.normalized) + (playerY * transform.up.normalized) + (playerZ * transform.forward.normalized);
        MovePlayer(movementVector);
    }

    void OnMove(InputValue movement)
    {       
        Vector2 inputVector = movement.Get<Vector2>();
        
        playerX = inputVector.x;
        playerZ = inputVector.y;
    }

    void MovePlayer(Vector3 movementVector)
    {
        if(!charController.isGrounded)
        {
            playerY += GRAVITY * Time.deltaTime;
        }

        charController.Move(playerSpeed * Time.deltaTime * movementVector);
    }

    void OnJump()
    {
        if(charController.isGrounded)
        {
            playerY = jumpHeight;
        }
    }

    void OnCrouch()
    {
        isCrouching = !isCrouching;
        charController.height = isCrouching == true ? crouchHeight : playerHeight;
    }

    void OnSprint()
    {
        isRunning = !isRunning;
    }

    float GetPlayerSpeed()
    {
        if(isCrouching)
        {
            return isRunning == true ? crouchRunningSpeed : crouchSpeed;
        }

        return isRunning == true ? playerRunningSpeed : playerNormalSpeed;
    }
}   
