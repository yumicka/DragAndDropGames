using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DiskUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [HideInInspector] public PillarUI currentPillar;
    [HideInInspector] public Vector3 lastValidPosition;
    [SerializeField] public float diskSize = 1f;
    [HideInInspector] public PillarUI lastPillar;

    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffsetLocal;
    private int originalSiblingIndex;
    private bool isDragging = false;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Canvas c = GetComponentInParent<Canvas>();
        if (c == null)
            Debug.LogError("DiskUI not found in Canvas!");
        canvas = c;
        lastValidPosition = rect.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = false;

       
        PillarUI pillar = PillarUI.FindPillarContainingDisk(this);

        if (pillar == null)
        {
            Debug.Log($"BeginDrag {name}: no pillar contains this disk");
            return; 
        }

        if (pillar.PeekTopDisk() != this)
        {
            Debug.Log($"BeginDrag {name}: not top disk on pillar {pillar.name}");
            return; 
        }

        isDragging = true;
        lastPillar = pillar;
        currentPillar = null;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.85f;

        lastValidPosition = rect.position;

        originalSiblingIndex = rect.GetSiblingIndex();
        rect.SetAsLastSibling();

        Vector2 localPointer;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rect.parent, eventData.position, canvas.worldCamera, out localPointer);
        dragOffsetLocal = rect.localPosition - (Vector3)localPointer;

        pillar.PopTopDisk();
        Debug.Log($"BeginDrag {name}: popped from pillar {pillar.name}");
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 localPointer;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)rect.parent, eventData.position, canvas.worldCamera, out localPointer))
        {
            rect.localPosition = (Vector3)localPointer + (Vector3)dragOffsetLocal;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Vector2 screenPos = eventData.position;
        PillarUI pillar = PillarUI.FindPillarUnderScreenPoint(screenPos, canvas);

        if (pillar != null && pillar.CanPlaceDisk(this))
        {
            pillar.PushDisk(this, snapImmediately: false, keepPosition: false);
            currentPillar = pillar;
            lastValidPosition = rect.position;
            Debug.Log($"EndDrag {name}: placed on pillar {pillar.name}");

            var gm = FindObjectOfType<GameManagerUI>();
            if (gm != null)
                gm.OnDiskPlaced(pillar);
        }
        else
        {
            if (lastPillar != null)
            {
                lastPillar.PushDisk(this, snapImmediately: true, keepPosition: false);
                currentPillar = lastPillar;
                Debug.Log($"EndDrag {name}: returned to pillar {lastPillar.name}");
            }

            rect.SetSiblingIndex(originalSiblingIndex);
        }
    }
}
