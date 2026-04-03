using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHudController : MonoBehaviour
{

    public Texture2D fullHeartImage;
    public Texture2D emptyHeartImage;
    private PlayerController playerInfo;
    private UIDocument document;
    private VisualElement root;
    
    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        repaint();
    }

    public void repaint()
    {

        VisualElement mainRow = root.Query(name: "heart-row");

        for(int i = 0; i < playerInfo.GetPlayerLife(); i++)
        {
            Image r = new Image();

            r.name = "heart";
            r.image = fullHeartImage;
            
            mainRow.Add(r);
        }
    }


}
