using UnityEngine;

public class CollectableController : MonoBehaviour
{
    
    public float collectableScore = 1f;

    [SerializeField] private ScoreManager score;

    void Start()
    {
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    public void Collect()
    {
        score.comunicateColletableCollected(gameObject);
        Destroy(gameObject);
    }

}   