using System;
using UnityEngine;
using UnityEngine.AI;

/*Este código foi feito com a pré suposição de que o unity do usuário possua o AI navegator.
Caso o usuário não tenha o pacakge (ou queira conferir), siga as seguintes instruções:
dentro do editor, Window-> Package Management-> package manager-> Unity Registry. Search: "AI Navigation"
Este script é inserido no inimigo(enemy) e 
*/
public class EnemyController : MonoBehaviour
{
    //lidando com knockback 
    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f;
    public float knockbackDuration = 0.3f;
    [SerializeField] private float knockbackForce = 5f;


    public float enemyPontuation = 1f;

    //Controle de dano que o inimigo causa ao player
    [SerializeField] private float attackRange = 2f; 
    [SerializeField] private float attackCooldown = 1f; 
    [SerializeField] private float enemyLife = 1f;
    [SerializeField] private float detectionRadius = 10f;
    private float lastAttackTime = 0f;
    private ScoreManager score;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Camera playerCamera;
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        player = GameObject.FindWithTag("Player").transform;
        playerCamera = Camera.main;
    }

    void Update()
    {
        knockback();
        TryAttackPlayer();
        MoveEnemy();
        RotateEnemy();
    }

    void MoveEnemy()
    {
        float distance =  Vector3.Distance(transform.position, player.position);

        //começa a conferir se o agente esta ativo por conta do sistema de knockback
        if(agent.enabled && distance <= detectionRadius)
        {
            agent.SetDestination(player.position);
        }
        else if(agent.enabled)
        {
            agent.ResetPath();
        }

        animator.SetBool("IsMoving",agent.velocity.magnitude > 0.1f);
    }

    void RotateEnemy()
    {
        transform.rotation = playerCamera.transform.rotation;        
    }

    void TryAttackPlayer()
    {
        float distance = Vector3.Distance(transform.position,player.position);

        if (distance <= attackRange)
        {
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if(playerController != null)
                {
                    playerController.TakeDamage(transform.position);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    public void TakeDamage()
    {
        enemyLife -= 1;
        animator.SetTrigger("WasHit");

        //Calcula direção e duração do knockback
        knockbackDirection = (transform.position - player.position).normalized;
        knockbackTimer = knockbackDuration;
        knockbackDirection.y = 0f;
        
        if(enemyLife == 0)
        {
            Destroy(gameObject);
            score.comunicateEnemyDeath(gameObject);
        }
    }

    public float GetEnemyLife()
    {
        return enemyLife;
    }


    //aplica o knockback
    void knockback()
    {
        if(knockbackTimer > 0)
        {
            agent.enabled = false;
            transform.Translate(knockbackDirection * knockbackForce * Time.deltaTime, Space.World);
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            agent.enabled = true;
        }
    }
}