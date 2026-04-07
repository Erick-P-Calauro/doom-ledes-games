using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string sceneName = "Scene01";

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(sceneName);
    }
}