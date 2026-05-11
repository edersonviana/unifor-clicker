using UnityEngine;
using UnityEngine.EventSystems;

public class ClickController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicou!");
        GameManager.Instance.Clicar();
    }
}