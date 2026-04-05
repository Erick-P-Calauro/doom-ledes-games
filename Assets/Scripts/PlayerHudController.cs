using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    [SerializeField] private Texture2D fullHeartImage;
    [SerializeField] private Texture2D emptyHeartImage;
    [SerializeField] private Texture2D playerFullLifeIcon;
    [SerializeField] private Texture2D playerMidLifeIcon;
    [SerializeField] private Texture2D playerLowLifeIcon;
    private PlayerController player;
    private UIDocument document;
    private VisualElement root;
    private VisualElement mainRow;
    private Image playerState;
    private ProgressBar progressBar;
    
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        mainRow = root.Query(name: "heart-row");
        progressBar = root.Query<ProgressBar>(name: "progress-sujeira");
        playerState = root.Query<Image>(name: "playerState");

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

        int life = player.GetPlayerLife();
        int maxLife = player.GetPlayerMaxLife();

        float midLife = maxLife * 0.75f; 
        float lowLife = maxLife * 0.35f; 

        // Se a diferença for negativa, então o player tomou o máximo de dano possível
        int lifeDif = (maxLife - life) > maxLife ? maxLife : (maxLife - life);

        for(int i = 0; i < life; i++)
        {
            mainRow.Add(GenerateHeart(true));
        }

        for(int i = 0; i < lifeDif; i++)
        {
            mainRow.Add(GenerateHeart(false));
        }

        if(life <= lowLife)
        {
            playerState.image = playerLowLifeIcon;
            return;
        }

        if(life <= midLife)
        {
            playerState.image = playerMidLifeIcon;
        }else
        {
            playerState.image = playerFullLifeIcon;
        }
    }

    public void repaintPoints()
    {
        int pointsPercentage = score.getPontuationPercentage();
        progressBar.value = pointsPercentage;

        score.comunicateHudChanged();
    }


}
