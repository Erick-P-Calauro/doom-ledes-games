using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    [SerializeField] private Texture2D fullHeartImage;
    [SerializeField] private Texture2D emptyHeartImage;
    private PlayerController playerInfo;
    private UIDocument document;
    private VisualElement root;
    private VisualElement mainRow;
    private ProgressBar progressBar;
    
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;

        mainRow = root.Query(name: "heart-row");
        progressBar = root.Query<ProgressBar>(name: "progress-sujeira");
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        repaintLife();
        repaintPoints();
    }

    void Update()
    {
        if(score.shouldChangeHud)
        {
            repaintPoints();
        }
    }

    public void repaintLife()
    {
        for(int i = 0; i < playerInfo.GetPlayerLife(); i++)
        {
            Image r = new Image();

            r.name = "heart";
            r.image = fullHeartImage;
            
            mainRow.Add(r);
        }
    }

    public void repaintPoints()
    {
        int pointsPercentage = score.getPontuationPercentage();
        progressBar.value = pointsPercentage;

        score.comunicateHudChanged();
    }


}
