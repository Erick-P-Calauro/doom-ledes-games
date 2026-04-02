using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudPainter : MonoBehaviour
{

    public Texture2D fullHeartImage;
    public Texture2D emptyHeartImage;
    private UIDocument document;
    private PlayerController playerInfo;
    
    void Start()
    {
        document = GetComponent<UIDocument>();
        playerInfo = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        paint();
    }

    // Definir como vai ser a pontuação
    VisualElement paintPointElements ()
    {
        Label l = new Label("Points : 0/150");

        l.style.color = Color.white;
        l.style.fontSize = 16f;
        l.style.paddingLeft = 8f;

        return l;
    }

    VisualElement generateHeart(bool isFull)
    {
        Image h = new Image();
        h.image = isFull ? fullHeartImage : emptyHeartImage;

        h.style.height = 24f;
        h.style.width = 24f;

        return h;
    }

    VisualElement generateRow(float left, float right, float top, float bottom)
    {
        VisualElement row = new VisualElement();
        
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;

        row.style.paddingTop = top;
        row.style.paddingLeft = left;
        row.style.paddingRight = right;
        row.style.paddingBottom = bottom;

        return row;
    }

    public void paint()
    {
        var root = document.rootVisualElement;

        VisualElement row = generateRow(8f, 0f, 10f, 0f);

        for(int i = 0; i < playerInfo.GetPlayerLife(); i++)
        {
            row.Add(generateHeart(true));
        }

        for(int i = 0; i < playerInfo.GetPlayerMaxLife() - playerInfo.GetPlayerLife(); i++)
        {
            row.Add(generateHeart(false));
        }

        row.Add(paintPointElements());
        root.Add(row);
    }
}
