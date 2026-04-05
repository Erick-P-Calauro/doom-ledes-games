using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    [SerializeField] private Texture2D fullHeartImage;
    [SerializeField] private Texture2D emptyHeartImage;
    private PlayerController player;
    private UIDocument document;
    private VisualElement root;
    private VisualElement mainRow;
    private ProgressBar progressBar;
    
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        mainRow = root.Query(name: "heart-row");
        progressBar = root.Query<ProgressBar>(name: "progress-sujeira");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        repaintLife();
        repaintPoints();
    }

    void Update()
    {
        if(score.shouldChangeHud)
        {
            repaintPoints();
        }

        if(player.DamageTaken())
        {
            repaintLife();
            player.ResetDamageTaken();
        }
    }

    VisualElement GenerateHeart(bool isFull)
    {
        Image r = new Image();

        r.name = "heart";
        r.image = isFull ? fullHeartImage : emptyHeartImage;

        return r;
    }

    public void repaintLife()
    {
        mainRow.Clear();

        for(int i = 0; i < player.GetPlayerLife(); i++)
        {
            mainRow.Add(GenerateHeart(true));
        }

        for(int i = 0; i < player.GetPlayerMaxLife() - player.GetPlayerLife(); i++)
        {
            mainRow.Add(GenerateHeart(false));
        }
    }

    public void repaintPoints()
    {
        int pointsPercentage = score.getPontuationPercentage();
        progressBar.value = pointsPercentage;

        score.comunicateHudChanged();
    }


}
