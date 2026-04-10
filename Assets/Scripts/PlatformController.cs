using System;
using UnityEngine;
public class PlatformController : MonoBehaviour
{
    [SerializeField] private float distance = 30f;
    [SerializeField] private float timeMoving = 5f; // Seconds
    [SerializeField] private float restartMoveDelay = 2f; // Seconds
    private bool isMoving = false;
    private bool isMovingForward = true;
    private DateTime lastStartMove = DateTime.Now;
    private DateTime lastEndMove = DateTime.Now;

    void Update()
    {
        if(isMoving)
        {
            if(DateTime.Now.Subtract(lastStartMove).Seconds <= timeMoving)
            {
                if(isMovingForward)
                {
                    transform.position += Time.deltaTime * (distance/timeMoving) * Vector3.right ;
                }else
                {
                    transform.position += Time.deltaTime  * -(distance/timeMoving) * Vector3.right;
                }

            }else
            {
                lastEndMove = DateTime.Now;
                isMovingForward = !isMovingForward;
                isMoving = false;
            }

        }else if (DateTime.Now.Subtract(lastEndMove).Seconds > restartMoveDelay)
        {
            isMoving = true;
            lastStartMove = DateTime.Now;
        }
    }
}