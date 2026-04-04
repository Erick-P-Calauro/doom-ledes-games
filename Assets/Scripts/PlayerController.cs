using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
    public AnimatorController playerAttack;
    public AnimatorController playerWalk;

    // Campos marcados com SerializeField podem ser observados pelo Inspector
    [SerializeField] private float playerLife = 3f;
    [SerializeField] private float playerMaxLife = 3f;
    [SerializeField] private float playerNormalSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float crouchRunningSpeed = 3f;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerAttackRange = 2f;
    [SerializeField] private ScoreManager score;

    // Campos sem SerializeField são estado interno do Player;
    private CharacterController charController;
    private Animator playerHandAnimator;
    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -9.81f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool isAttacking = false;

    public bool isDamaging = false;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        
        playerSpeed = playerNormalSpeed;
        charController.height = playerHeight;
        
        playerHandAnimator = GameObject.FindGameObjectWithTag("CameraAttach").GetComponent<Animator>();
        playerHandAnimator.speed = 0;
    }

    void Update()
    {   
        playerSpeed = GetPlayerSpeed();
        
        // Precisa levar em conta up, right e forward (eixos no espaço) para se mover com base na rotação da câmera
        Vector3 movementVector = (playerX * transform.right.normalized) + (playerY * transform.up.normalized) + (playerZ * transform.forward.normalized);
        MovePlayer(movementVector);
        
        RefreshAttackState();
        RefreshAnimationState();
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

    void OnAttack() {
        isAttacking = true;
        isDamaging = true;
        playerHandAnimator.runtimeAnimatorController = playerAttack;
    }

    void RefreshAttackState()
    {
        if(!isAttacking)
        {
            playerHandAnimator.runtimeAnimatorController = playerWalk;
            return;
        }

        if(playerHandAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            isAttacking = false;
        }
        
        if(isDamaging && playerHandAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.22)
        {
            ComputateAttack();
            isDamaging= false;
        }
    }


    void ComputateAttack()
    {
        Collider[] collidersNearby = Physics.OverlapSphere(transform.position, playerAttackRange);
        
        float bestAngle = 41f;
        Collider bestTarget = null;

        foreach(Collider c in collidersNearby)
        {
            if(!c.CompareTag("Enemy"))
            {
                continue;
            }

            // Usa o centro do collider e não do gameobject do enemy.
            Vector3 directionToColider = (c.transform.position - transform.position).normalized;
            float angleBetweenObjects = Vector3.Angle(transform.forward, directionToColider);
            
            // Lógica para pegar o alvo mais adequado à mira do jogador 
            if(angleBetweenObjects <= 40f && bestAngle > angleBetweenObjects)
            {
                bestTarget = c;
                bestAngle = angleBetweenObjects;
            }
        }

        if(bestTarget != null)
        {
            GameObject enemy = bestTarget.gameObject;
            EnemyController enemyController = enemy.GetComponent<EnemyController>();

            enemyController.TakeDamage();
        }
    }

    float GetPlayerSpeed()
    {
        if(isCrouching)
        {
            return isRunning == true ? crouchRunningSpeed : crouchSpeed;
        }

        return isRunning == true ? playerRunningSpeed : playerNormalSpeed;
    }

    void RefreshAnimationState()
    {
        if(isAttacking)
        {
            playerHandAnimator.speed = 1;
            return;
        }

        if(charController.velocity.magnitude == 0)
        {
            playerHandAnimator.speed = 0;
        }else
        {
            playerHandAnimator.speed = playerSpeed/playerNormalSpeed;
        }
    }

    public void TakeDamage()
    {
        playerLife -= 1;
    }

    public float GetPlayerLife()
    {
        return playerLife;
    }

    public float GetPlayerMaxLife()
    {
        return playerMaxLife;    
    }
}   
