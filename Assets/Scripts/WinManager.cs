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

            ScoreData.scoreValues = score.getScoreValues();
            SceneManager.LoadScene("WinScene");
        }
    }

}