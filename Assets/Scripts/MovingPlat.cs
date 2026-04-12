
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class MovingPlat : MonoBehaviour
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
    
    void Start()
    {
        Platform.transform.position = PointA.transform.position;
        targetPosition = PointB.transform.position;
        StartCoroutine(MovePlatform());
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
            other.transform.SetParent(platformTransform);
        }
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
