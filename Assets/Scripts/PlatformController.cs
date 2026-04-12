
using UnityEngine;
using System.Collections;


public class PlatformController : MonoBehaviour
{
    //Mover plataforma
    [SerializeField] GameObject PointA;
    [SerializeField] GameObject PointB;
    [SerializeField] float speed = 10f;
    [SerializeField] float delay = 1f;
    [SerializeField] GameObject Platform;
    private Vector3 targetPosition;

    //Manter o player encima da plataforma
    [SerializeField] Transform platformTransform;
    private Transform playerTransform;
    private Vector3 lastPlatformPosition;
    
    void Start()
    {
        Platform.transform.position = PointA.transform.position;
        targetPosition = PointB.transform.position;
        lastPlatformPosition = Platform.transform.position;
        StartCoroutine(MovePlatform());
    }


    void Update()
    {
        if(playerTransform != null)
        {
            Vector3 delta = Platform.transform.position - lastPlatformPosition;
            playerTransform.position += delta*2.5f;
        }
        lastPlatformPosition = Platform.transform.position;
    }
    IEnumerator MovePlatform()
    {
        while (true)
        {
            while((targetPosition - Platform.transform.position).sqrMagnitude > 0.01f)
            {
                Platform.transform.position = Vector3.MoveTowards(Platform.transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
            targetPosition = targetPosition == PointA.transform.position
                ? PointB.transform.position : PointA.transform.position;
                yield return new WaitForSeconds(delay);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }
}
