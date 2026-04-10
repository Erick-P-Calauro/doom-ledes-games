using UnityEngine;

public class VenenoCollider : MonoBehaviour
{
    private float timer = 0f;
    private float seconds = 2.0f;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if(timer >= seconds)
            {
                other.GetComponent<PlayerController>().TakeDamage(transform.position);
                timer = 0f;
            }
        }
    }
}
