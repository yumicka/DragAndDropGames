using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrowScript : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGro;
    private RectTransform rectTra;
    public ObjectScript objectScr;
    void Start()
    {
        canvasGro = GetComponent<CanvasGroup>();
        rectTra = GetComponent<RectTransform>(); 
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if((Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetMouseButton(2))) {
            Debug.Log("OnPoinerDown");
            objectScr.effects.PlayOneShot(objectScr.audioCli[0]);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
