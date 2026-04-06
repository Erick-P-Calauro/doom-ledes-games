using UnityEngine;
using UnityEngine.EventSystems;

public class LeaveController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}