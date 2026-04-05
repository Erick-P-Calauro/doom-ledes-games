using UnityEngine;
using UnityEngine.AI;

public class EnemiesBoolParameters : MonoBehaviour
{

    private NavMeshAgent agent;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsMoving",agent.velocity.magnitude > 0.1f);
    }
}

