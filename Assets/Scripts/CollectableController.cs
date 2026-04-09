using UnityEngine;

public class CollectableController : MonoBehaviour
{
    
    public float collectableScore = 1f;
    [SerializeField] private CollectablesEnum category;
    [SerializeField] private ScoreManager score;
    [SerializeField] private InventoryManager inventory;

    void Start()
    {
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        inventory = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    public void Collect()
    {
        score.comunicateColletableCollected(gameObject);
        inventory.AddItem(category);
        Destroy(gameObject);
    }

}   