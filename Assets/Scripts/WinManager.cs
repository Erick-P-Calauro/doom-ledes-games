using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    private GameObject player;
    private ScoreManager score;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        Vector3 diff = player.transform.position - transform.position;

        if(diff.magnitude <= 5f)
        {   
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CreateAndSaveScoreAsset();

            SceneManager.LoadScene("WinScene");
        }
    }

    void CreateAndSaveScoreAsset()
    {
        var score_values = score.getScoreValues();
        PlayerScoreData data =  ScriptableObject.CreateInstance<PlayerScoreData>();
        
        data.enemiesScore = score_values["enemies"];
        data.totalScore = score_values["total"];
        
        UnityEditor.AssetDatabase.CreateAsset(data, "Assets/score_data.asset");
    }

}