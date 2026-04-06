using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private RawImage select;

    private bool isSelected = false;

    void Update()
    {
        if(isSelected)
        {
            select.gameObject.SetActive(true);
        }else
        {
            select.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isSelected = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isSelected = false;
    }
}
