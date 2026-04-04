using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

/*Este código foi feito com a pré suposição de que o unity do usuário possua o AI navegator.
Caso o usuário não tenha o pacakge (ou queira conferir), siga as seguintes instruções:
dentro do editor, Window-> Package Management-> package manager-> Unity Registry. Search: "AI Navigation"
Este script é inserido no inimigo(enemy) e 
*/
public class EnemyController : MonoBehaviour
{

    public float enemyPontuation = 1f;

    //Controle de dano que o inimigo causa ao player
    [SerializeField] private float attackRange = 2f; 
    [SerializeField] private float attackCooldown = 1f; 
    private float lastAttackTime = 0f;

    [SerializeField] private float enemyLife = 1f;
    [SerializeField] private float detectionRadius = 10f;
    private ScoreManager score;
    private Transform player;
    private NavMeshAgent agent;
    private Camera playerCamera;
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        try
        {
            score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        }catch(Exception e)
        {
            Debug.Log("Enemy Exception : " + e.ToString());
        }

        player = GameObject.FindWithTag("Player").transform;
        playerCamera = Camera.main;
    }

    void Update()
    {
        TryAttackPlayer();
        MoveEnemy();
        RotateEnemy();
    }

    void MoveEnemy()
    {
        float distance =  UnityEngine.Vector3.Distance(transform.position, player.position);
        if(distance <= detectionRadius)
        {
            agent.SetDestination(player.position);   
        }
        else
        {
            agent.ResetPath();
        }
    }

    void RotateEnemy()
    {
        transform.rotation = playerCamera.transform.rotation;        
    }

    public void TakeDamage()
    {
        enemyLife -= 1;
        
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

    void TryAttackPlayer()
    {
        Debug.Log("Tentando atacar");
        float distance = UnityEngine.Vector3.Distance(transform.position,player.position);

        if (distance <= attackRange)
        {
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if(playerController != null)
                {
                    Debug.Log("ATACOU!");
                    playerController.TakeDamage();
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}