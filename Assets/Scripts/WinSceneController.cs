using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class WinSceneController : MonoBehaviour
{
    public PlayerScoreData data;

    [SerializeField] private TextMeshProUGUI enemies_text;
    [SerializeField] private TextMeshProUGUI collectables_text;
    [SerializeField] private TextMeshProUGUI total_text;

    void Start()
    {     
        try
        {            
            enemies_text.text = "Inimigos Derrotados : +" + data.enemiesScore + " Pontos.";
            collectables_text.text = "Lixo Coletado : +" + data.collectablesScore + " Pontos.";
            total_text.text = "Total : +" + data.totalScore + " Pontos.";
            
        }catch(Exception e)
        {
            Debug.Log("Score Data error " + e.ToString() + ".");
        }
    }
}
