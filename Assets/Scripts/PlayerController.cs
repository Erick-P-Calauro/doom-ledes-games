using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{   
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
    private Animator playerAnimator;
    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -6f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool isAttacking = false;

    public bool isDamaging = false;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        
        playerSpeed = playerNormalSpeed;
        charController.height = playerHeight;
        
        playerAnimator = GameObject.FindGameObjectWithTag("CameraAttach").GetComponent<Animator>();
        
        RefreshAnimatorState();
    }

    void Update()
    {   
        playerSpeed = GetPlayerSpeed();
        
        // Precisa levar em conta up, right e forward (eixos no espaço) para se mover com base na rotação da câmera
        Vector3 movementVector = (playerX * transform.right.normalized) + (playerY * transform.up.normalized) + (playerZ * transform.forward.normalized);
        MovePlayer(movementVector);
        
        RefreshAttackState();
        RefreshAnimatorState();
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
        if(isAttacking)
        {
            return;
        }

        playerAnimator.ResetTrigger("Attack");
        playerAnimator.SetTrigger("Attack");
        isAttacking = true;
        isDamaging = true;
    }

    void RefreshAttackState()
    {
        if(!isAttacking)
        {
            return;
        }

        AnimatorStateInfo state = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if(state.IsName("Attack") && state.normalizedTime > 1f)
        {
            isAttacking = false;
            playerAnimator.ResetTrigger("Attack");
        }
        
        if(isDamaging && state.IsName("Attack") && state.normalizedTime >= 0.28)
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

    void RefreshAnimatorState()
    {
        playerAnimator.SetBool("IsWalking",  charController.velocity.magnitude > 0);
        playerAnimator.SetBool("IsRunning", isRunning);
        playerAnimator.SetBool("IsCrouching", isCrouching);
    }

    float GetPlayerSpeed()
    {
        if(isCrouching)
        {
            return isRunning == true ? crouchRunningSpeed : crouchSpeed;
        }

        return isRunning == true ? playerRunningSpeed : playerNormalSpeed;
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