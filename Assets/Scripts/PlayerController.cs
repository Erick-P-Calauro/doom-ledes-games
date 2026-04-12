using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour
{   
    // Campos marcados com SerializeField podem ser observados pelo Inspector
    [SerializeField] private int playerLife = 3;
    [SerializeField] private int playerMaxLife = 3;
    [SerializeField] private float playerNormalSpeed = 5f;
    [SerializeField] private float playerRunningSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float crouchRunningSpeed = 3f;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerAttackRange = 2f;
    [SerializeField] private double playerAttackDelay = 100; // Milliseconds
    [SerializeField] private float knockbackForce = 25f;
    [SerializeField] private Vector3 knockbackVelocity;
    [SerializeField] private AudioClip attackingSoundClip;
    [SerializeField] private AudioClip damageDealSoundClip;
    [SerializeField] private AudioClip collectTrashSoundClip;
    // Campos sem SerializeField são estado interno do Player;
    private CharacterController charController;
    private Animator playerAnimator;
    private AudioSource attackingSound;
    private AudioSource damageDealSound;
    private AudioSource collectTrashSound;
    private float playerX = 0f;
    private float playerY = 0f;
    private float playerZ = 0f;
    private const float GRAVITY = -6f;
    private bool isCrouching = false;
    private bool isRunning = false;
    private bool isAttacking = false;
    private DateTime lastAttack = DateTime.MinValue;
    private bool isDamaging = false;
    private bool damageTaken = false;
    private bool healTaken = false;
    private bool damageDeal = false;
    
    //Variáveis do sistema de dash
    private bool dashing = true;
    private float dashingPower = 20f;
    private float dashTime = 0.3f;
    private float dashCooldown = 0.45f;


    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerAnimator = GameObject.FindGameObjectWithTag("CameraAttach").GetComponent<Animator>();

        playerSpeed = playerNormalSpeed;
        charController.height = playerHeight;
        
        UpdateAnimatorState();
        StartSoundComponents();
    }

    void StartSoundComponents()
    {
        attackingSound = gameObject.AddComponent<AudioSource>();
        attackingSound.clip = attackingSoundClip;
        attackingSound.loop = false;

        damageDealSound = gameObject.AddComponent<AudioSource>();
        damageDealSound.clip = damageDealSoundClip;
        damageDealSound.loop = false;

        collectTrashSound = gameObject.AddComponent<AudioSource>();
        collectTrashSound.clip = collectTrashSoundClip;
        collectTrashSound.loop = false;
    }

    void Update()
    {   
        UpdatePosition();
        UpdateAttackState();
        UpdateAnimatorState();
        UpdateSoundEffects();
    }

    void UpdatePosition()
    {
        playerSpeed = GetPlayerSpeed();
        
        // Precisa levar em conta up, right e forward (eixos no espaço) para se mover com base na rotação da câmera
        Vector3 movementVector = (playerX * transform.right.normalized) + (playerY * transform.up.normalized) + (playerZ * transform.forward.normalized);
        MovePlayer(movementVector);
    }

    void UpdateAttackState()
    {
        if(!isAttacking)
        {
            return;
        }

        AnimatorStateInfo state = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if(state.IsName("Attack") && state.normalizedTime > 1f)
        {
            playerAnimator.ResetTrigger("Attack");
            isAttacking = false;
            damageDeal = false;
            lastAttack = DateTime.Now;
        }
        
        if(isDamaging && state.IsName("Attack") && state.normalizedTime >= 0.28)
        {
            ComputateAttack();
            isDamaging = false;
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
            damageDeal = true;
        }
    }

    void UpdateAnimatorState()
    {
        playerAnimator.SetBool("IsWalking",  charController.velocity.magnitude > 0);
        playerAnimator.SetBool("IsRunning", isRunning);
        playerAnimator.SetBool("IsCrouching", isCrouching);
    }

    void UpdateSoundEffects()
    {
        AnimatorStateInfo state = playerAnimator.GetCurrentAnimatorStateInfo(0);

        // A utilização do animator state garante sincronicidade com a animação.
        if(isAttacking && !attackingSound.isPlaying && state.IsName("Attack") && state.normalizedTime > 0f)
        {
            attackingSound.Play();
        }

        if(damageDeal && !damageDealSound.isPlaying)
        {
            damageDealSound.Play();
        }
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
        
        Vector3 finalMove = movementVector * playerSpeed;
        finalMove += knockbackVelocity;

        charController.Move(finalMove * Time.deltaTime);

        knockbackVelocity = Vector3.Lerp(knockbackVelocity,Vector3.zero, 5f * Time.deltaTime);
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
        if(isAttacking || (DateTime.Now.Subtract(lastAttack).TotalMilliseconds < playerAttackDelay))
        {
            return;
        }

        playerAnimator.ResetTrigger("Attack");
        playerAnimator.SetTrigger("Attack");
        
        isAttacking = true;
        isDamaging = true;
    }

    void CollectTrash(GameObject trash)
    {
        var c = trash.GetComponent<CollectableController>();
        c.Collect();
        collectTrashSound.Play();
    }

    void CollectMedkit(GameObject medkit)
    {
        if(playerLife != playerMaxLife) 
        {
            playerLife += 1;  
            healTaken = true;
            Destroy(medkit);
            collectTrashSound.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if(otherObject.CompareTag("Collectable"))
        {
            CollectTrash(otherObject);
        }

        if(otherObject.CompareTag("Medkit"))
        {
            CollectMedkit(otherObject);
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

    public void TakeDamage(Vector3 attackerPosition)
    {
        playerLife -= 1;
        
        if(playerLife == 0)
        {
            Die();
        }

        //Calcula o vetor direção e o normaliza(1), aplica o knockback
        Vector3 direction = (transform.position - attackerPosition).normalized;
        knockbackVelocity = direction * knockbackForce;

        damageTaken = true;
    }

    private void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadSceneAsync("DieScene");
    }

    public int GetPlayerLife()
    {
        return playerLife;
    }

    public int GetPlayerMaxLife()
    {
        return playerMaxLife;    
    }

    public bool LifeChanged()
    {
        return damageTaken || healTaken;
    }

    public void ResetLifeChange()
    {
        damageTaken = false;
        healTaken = false;
    }


    void OnDash()
    {
        if (dashing)
        {
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        dashing = false;    
        Vector3 dashDirection =(playerX * transform.right + playerZ * transform.forward).normalized;
        float timer = 0f;
        while (timer < dashTime)
        {
            charController.Move(dashDirection * dashingPower * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
            
        }
        yield return new WaitForSeconds(dashCooldown);
        dashing = true;
    }
}   