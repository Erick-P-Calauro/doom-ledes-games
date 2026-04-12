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
    //controla o delay para destruir um objeto após sua última vida 
    public float deathDelay = 1.0f;
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
    [SerializeField] private double idleAudioDelay = 4500; // Milliseconds.
    [SerializeField] private AudioClip idleAudioClip;
    [SerializeField] private AudioClip damageTakenAudioClip;
    private AudioSource idleAudio;
    private AudioSource damageTakenAudio;
    private float lastAttackTime = 0f;
    private ScoreManager score;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Camera playerCamera;
    private bool damageTaken = false;

    private DateTime lastIdleSound = DateTime.MinValue;
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        player = GameObject.FindWithTag("Player").transform;
        playerCamera = Camera.main;

        StartSoundEffects();
    }

    void StartSoundEffects()
    {
        idleAudio = gameObject.AddComponent<AudioSource>();
        idleAudio.loop = false;
        idleAudio.clip = idleAudioClip;
        idleAudio.volume = 0.7f;

        damageTakenAudio = gameObject.AddComponent<AudioSource>();
        damageTakenAudio.loop = false;
        damageTakenAudio.clip = damageTakenAudioClip;
    }
    void Update()
    {
        Knockback();
        TryAttackPlayer();
        MoveEnemy();
        RotateEnemy();
        UpdateSoundEffects();
    }

    void UpdateSoundEffects()
    {
        if(DateTime.Now.Subtract(lastIdleSound).TotalMilliseconds > idleAudioDelay && !idleAudio.isPlaying && agent.velocity.magnitude > 0.1f)
        {
            idleAudio.Play();
            lastIdleSound = DateTime.Now;
        }

        if(!damageTakenAudio.isPlaying && damageTaken)
        {
            damageTakenAudio.Play();
            damageTaken = false;
        }
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
        transform.rotation = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0);   
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
        damageTaken = true;

        //Calcula direção e duração do knockback
        knockbackDirection = (transform.position - player.position).normalized;
        knockbackTimer = knockbackDuration;
        knockbackDirection.y = 0f;
        
        if(enemyLife == 0)
        {
            UpdateSoundEffects();
            score.comunicateEnemyDeath(gameObject);
            //adicionado um delay para destruir o objeto "enemy"
            Destroy(gameObject, deathDelay);
        }
    }

    public float GetEnemyLife()
    {
        return enemyLife;
    }

    //aplica o knockback
    void Knockback()
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