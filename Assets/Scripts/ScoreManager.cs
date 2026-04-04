using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool shouldChangeHud = false;
    [SerializeField] private float scoreAvailable;
    [SerializeField] private float scoreTotal;

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        scoreTotal = 0f;

        foreach(GameObject e in enemies)
        {
            scoreTotal += e.GetComponent<EnemyController>().enemyPontuation;
        }

        scoreAvailable = scoreTotal;

        shouldChangeHud = true;
    }

    public void comunicateHudChanged()
    {
        shouldChangeHud = false;
    }

    public void comunicateEnemyDeath(GameObject enemy)
    {
        scoreAvailable -= enemy.GetComponent<EnemyController>().enemyPontuation;
        shouldChangeHud = true;
    }

    public int getPontuationPercentage()
    {
        return (int) Math.Floor(scoreAvailable / scoreTotal * 100);
    }
}
