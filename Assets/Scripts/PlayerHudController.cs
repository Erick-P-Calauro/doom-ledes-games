using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private ScoreManager score;
    [SerializeField] private InventoryManager inventoryManager;
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
    private VisualElement rowInventory;
    
    void Start()
    {        
        InitializeUIDocumentFields();
        InitializeComponents();
        repaintLife();
        repaintPoints();
        repaintInventory(inventoryManager.GetInventory());
    }

    void InitializeUIDocumentFields()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;

        mainRow = root.Query(name: "heart-row");
        progressBar = root.Query<ProgressBar>(name: "progress-sujeira");
        playerState = root.Query<Image>(name: "playerState");
        rowInventory = root.Query<VisualElement>(name: "row-inventory");
    }

    void InitializeComponents()
    {
        score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if(score.shouldChangeHud)
        {
            repaintPoints();
        }

        if(inventoryManager.shouldChangeHud)
        {
            repaintInventory(inventoryManager.GetInventory());
        }

        if(player.LifeChanged())
        {
            repaintLife();
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

        player.ResetLifeChange();
    }

    public void repaintPoints()
    {
        int pointsPercentage = score.getPontuationPercentage();
        progressBar.value = pointsPercentage;

        score.comunicateHudChanged();
    }

    public void repaintInventory(Dictionary<CollectablesEnum, int> inventory)
    {
        foreach(VisualElement col in rowInventory.Children())
        {
            Label label = col.Query<Label>(name: "InventoryLabel");
            label.text = inventory[Enum.Parse<CollectablesEnum>(col.name)].ToString();
        }

        inventoryManager.ComunicateHudChanged();
    }
}
