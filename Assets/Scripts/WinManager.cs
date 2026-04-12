using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    public PlayerScoreData data;
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
        
        data.enemiesScore = score_values["enemies"];
        data.collectablesScore = score_values["collectables"];
        data.totalScore = score_values["total"];
    }

}