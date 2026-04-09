using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public bool shouldChangeHud = false;
    private float scoreAvailable;
    private float scoreTotal;
    
    // Peso para os pontos : 
    // Inimigos : 100
    // Coletáveis : 25
    private Dictionary<string, float> scoreValues = new Dictionary<string, float>();

    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        scoreValues.Add("enemies", 0);
        scoreValues.Add("total", 0);

        scoreTotal = 0f;

        foreach(GameObject e in enemies)
        {
            scoreTotal += e.GetComponent<EnemyController>().enemyPontuation * 100;
        }

        scoreAvailable = scoreTotal;
        shouldChangeHud = true;
    }

    public void comunicateHudChanged()
    {
        shouldChangeHud = false;
    }

    void RefreshTotalScore(float score)
    {
        scoreValues["total"] += score;
    }

    void RefreshEnemyScore(float score)
    {
        scoreValues["enemies"] += score;
        RefreshTotalScore(score);
    }

    public void comunicateEnemyDeath(GameObject enemy)
    {
        float enemyPontuation = enemy.GetComponent<EnemyController>().enemyPontuation * 100;

        scoreAvailable -= enemyPontuation;
        shouldChangeHud = true;

        RefreshEnemyScore(enemyPontuation);
    }

    public int getPontuationPercentage()
    {
        return (int) Math.Floor(scoreAvailable / scoreTotal * 100);
    }

    public Dictionary<string, float> getScoreValues()
    {
        return scoreValues;
    }
}
