using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Disk_drag_drop : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Pillar_script currentPeg;
    [HideInInspector] public Vector3 lastValidPosition;
    [HideInInspector] public float diskSize; // диаметр/размер диска для правил

    // Drag & Drop
    private bool isDragging = false;
    private Vector3 dragOffset;
    private Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        // Зафиксируем вращение
        rb.freezeRotation = true;
    }

    void Start()
    {
        lastValidPosition = transform.position;
    }

    void Update()
    {
        // --- Touch support ---
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector3 touchWorldPos = cam.ScreenToWorldPoint(t.position);
            touchWorldPos.z = 0f;

            switch (t.phase)
            {
                case TouchPhase.Began:
                    TryStartDrag(touchWorldPos);
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging) DragTo(touchWorldPos);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isDragging) EndDrag();
                    break;
            }
        }
    }

    // --- Начало перетаскивания ---
    private void TryStartDrag(Vector3 touchPos)
    {
        // Debug: покажем точку тача
        Debug.Log("TryStartDrag at: " + touchPos);

        // Получаем все коллайдеры в точке (на случай дочерних объектов)
        Collider2D[] hits = Physics2D.OverlapPointAll(touchPos);
        if (hits == null || hits.Length == 0)
        {
            Debug.Log("OverlapPointAll: nothing hit");
            return;
        }

        // Логируем, что было найдено (полезно для отладки)
        foreach (var h in hits)
        {
            Debug.Log("Overlap hit: " + h.name + " (gameObject: " + h.gameObject.name + ")");
        }

        // Ищем наш скрипт среди найденных коллайдеров (на том же объекте или у родителя)
        foreach (var h in hits)
        {
            Disk_drag_drop dd = h.GetComponentInParent<Disk_drag_drop>();
            if (dd != null && dd == this)
            {
                // Если верхний на пеге — начинаем перетаскивание
                if (currentPeg != null && currentPeg.PeekTopDisk() != this) return;
                StartDrag(touchPos);
                return;
            }
        }

        Debug.Log("No matching Disk_drag_drop component found under touch point.");
    }


    private void StartDrag(Vector3 touchPos)
    {
        isDragging = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        lastValidPosition = transform.position;
        dragOffset = transform.position - touchPos;

        if (currentPeg != null) currentPeg.PopTopDisk();
        currentPeg = null;
    }

    // --- Перетаскивание ---
    private void DragTo(Vector3 targetPos)
    {
        transform.position = targetPos + dragOffset;
    }

    // --- Конец перетаскивания ---
    private void EndDrag()
    {
        isDragging = false;

        Pillar_script peg = FindPegUnderPosition(transform.position);
        if (peg != null && peg.CanPlaceDisk(this))
        {
            Vector3 place = peg.GetTopPositionForDisk(this);
            transform.position = new Vector3(place.x, place.y + 0.3f, place.z);
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            StartCoroutine(FinalizePlacementCoroutine(peg, 0.2f));
        }
        else
        {
            ReturnToLastPosition();
        }
    }

    private IEnumerator FinalizePlacementCoroutine(Pillar_script peg, float wait)
    {
        yield return new WaitForSeconds(wait);
        transform.position = peg.GetTopPositionForDisk(this);
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        peg.PushDisk(this);
        currentPeg = peg;
        lastValidPosition = transform.position;
    }

    private void ReturnToLastPosition()
    {
        transform.position = lastValidPosition;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(FinalizeReturnCoroutine(0.05f));
    }

    private IEnumerator FinalizeReturnCoroutine(float wait)
    {
        yield return new WaitForSeconds(wait);
        rb.bodyType = RigidbodyType2D.Dynamic;

        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
        foreach (var c in hits)
        {
            Pillar_script p = c.GetComponentInParent<Pillar_script>();
            if (p != null)
            {
                p.PushDisk(this);
                currentPeg = p;
                lastValidPosition = transform.position;
                rb.bodyType = RigidbodyType2D.Kinematic;
                yield break;
            }
        }
    }

    // --- Поиск пега под диском ---
    private Pillar_script FindPegUnderPosition(Vector3 pos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);
        foreach (var c in hits)
        {
            Pillar_script p = c.GetComponentInParent<Pillar_script>();
            if (p != null) return p;
        }
        return Pillar_script.FindClosestPeg(pos);
    }
}
