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

    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffsetLocal;
    private int originalSiblingIndex;

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
        // only top disk allowed to drag
        if (currentPillar != null && currentPillar.PeekTopDisk() != this) return;

        // visual
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.85f;

        // remember last valid pos
        lastValidPosition = rect.position;

        // bring visually to front
        originalSiblingIndex = rect.GetSiblingIndex();
        rect.SetAsLastSibling();

        // compute offset so item doesn't jump
        Vector2 localPointer;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rect.parent, eventData.position, canvas.worldCamera, out localPointer);
        dragOffsetLocal = rect.localPosition - (Vector3)localPointer;

        // detach from pillar stack (if any)
        if (currentPillar != null) currentPillar.PopTopDisk();
        currentPillar = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointer;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)rect.parent, eventData.position, canvas.worldCamera, out localPointer))
        {
            rect.localPosition = (Vector3)localPointer + (Vector3)dragOffsetLocal;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rect.position);
        PillarUI pillar = PillarUI.FindPillarUnderScreenPoint(screenPos, canvas);

        if (pillar != null && pillar.CanPlaceDisk(this))
        {
            // Добавляем диск на верх стека с пересчётом позиции
            pillar.PushDisk(this, snapImmediately: false, keepPosition: false);
            currentPillar = pillar;
            lastValidPosition = rect.position;
        }
        else
        {
            // Возврат на предыдущую позицию
            StartCoroutine(MoveToPosition(rect.position, lastValidPosition, 0.12f, () =>
            {
                rect.SetSiblingIndex(originalSiblingIndex);
            }));
        }
    }



    IEnumerator MoveToPosition(Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            rect.position = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }
        rect.position = to;
        onComplete?.Invoke();
    }
}
