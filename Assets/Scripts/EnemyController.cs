using UnityEngine;
using UnityEngine.AI;

/*Este código foi feito com a pré suposição de que o unity do usuário possua o AI navegator.
Caso o usuário não tenha o pacakge (ou queira conferir), siga as seguintes instruções:
dentro do editor, Window-> Package Management-> package manager-> Unity Registry. Search: "AI Navigation"
Este script é inserido no inimigo(enemy) e 
*/
public class EnemyController : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    public float detectionRadius = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position,player.position);
        if(distance <= detectionRadius)
        {
            agent.SetDestination(player.position);   
        }
        else
        {
            agent.ResetPath();
        }        
    }
}
