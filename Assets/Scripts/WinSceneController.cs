using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class WinSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemies_text;
    [SerializeField] private TextMeshProUGUI total_text;

    void Start()
    {     
        try
        {
            var score_data = AssetDatabase.LoadAssetAtPath<PlayerScoreData>("Assets/score_data.asset");
            
            enemies_text.text = "Inimigos Derrotados : +" + score_data.enemiesScore + " Pontos.";
            total_text.text = "Total : +" + score_data.totalScore + " Pontos.";

            AssetDatabase.DeleteAsset("Assets/score_data.asset");
            
        }catch(Exception e)
        {
            Debug.Log("Score Data error " + e.ToString() + ".");
        }
    }
}
