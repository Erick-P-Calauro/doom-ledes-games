using UnityEngine;
using UnityEngine.AI;

/*Este código foi feito com a pré suposição de que o unity do usuário possua o AI navegator.
Caso o usuário não tenha o pacakge (ou queira conferir), siga as seguintes instruções:
dentro do editor, Window-> Package Management-> package manager-> Unity Registry. Search: "AI Navigation"
Este script é inserido no inimigo(enemy) e 
*/
public class EnemyController : MonoBehaviour
{

    [SerializeField] private float enemyLife = 1f;
    [SerializeField] private float detectionRadius = 10f;
    private Transform player;
    private NavMeshAgent agent;
    private Camera playerCamera;
   
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        playerCamera = Camera.main;
    }

    void Update()
    {
        MoveEnemy();
        RotateEnemy();
    }

    void MoveEnemy()
    {
        float distance = Vector3.Distance(transform.position, player.position);
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

    public bool TakeDamage()
    {
        enemyLife -= 1;
        if(enemyLife == 0)
        {
            Destroy(gameObject);
            return true;
        }
        
        return false;
    }

    public float GetEnemyLife()
    {
        return enemyLife;
    }
}
