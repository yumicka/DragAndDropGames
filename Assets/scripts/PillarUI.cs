using System.Collections.Generic;
using UnityEngine;

public class PillarUI : MonoBehaviour
{
    private Stack<DiskUI> disks = new Stack<DiskUI>();

    public float diskSpacing = 50f;

    
    public float baseYPosition = 0f;


    public RectTransform topAnchor; // если null, будет сам RectTransform этого объекта

    private static List<PillarUI> all = new List<PillarUI>();

    void Awake()
    {
        all.Add(this);
    }
    void OnDestroy()
    {
        all.Remove(this);
    }

    void Start()
    {
        // если diskSpacing не задан положительно, попробуем вычислить по первому дочернему диску (если есть)
        if (diskSpacing <= 0f)
        {
            DiskUI any = GetComponentInChildren<DiskUI>();
            if (any != null)
            {
                RectTransform dr = any.GetComponent<RectTransform>();
                // используем высоту rect как spacing (подойдёт при pivot (0.5,0) — нижняя точка)
                diskSpacing = dr.rect.height;
            }
            if (diskSpacing <= 0f) diskSpacing = 30f; // запасной шаг
        }

        // Если на пеге в инспекторе уже стоят диски (в иерархии), соберём их
        DiskUI[] existing = GetComponentsInChildren<DiskUI>(true);
        if (existing != null && existing.Length > 0)
        {
            // сортируем по anchored Y (снизу вверх)
            System.Array.Sort(existing, (a, b) =>
            {
                float ay = a.GetComponent<RectTransform>().anchoredPosition.y;
                float by = b.GetComponent<RectTransform>().anchoredPosition.y;
                return ay.CompareTo(by);
            });

            // Добавляем в стек в порядке снизу вверх (так индекс = 0 для нижнего)
            for (int i = 0; i < existing.Length; i++)
            {
                PushDisk(existing[i], snapImmediately: true, keepPosition: true);
            }
        }
        Debug.Log("Disks in pillar:");
        foreach (var d in disks)
        {
            Debug.Log(d.name + " size: " + d.diskSize);
        }
    }

    public DiskUI PeekTopDisk()
    {
        return disks.Count == 0 ? null : disks.Peek();
    }

    public DiskUI PopTopDisk()
    {
        return disks.Count == 0 ? null : disks.Pop();
    }

    // Публичное количество дисков
    public int DiskCount => disks.Count;

    // push: snapImmediately = true используется при инициализации сцены (не анимировать)
    public void PushDisk(DiskUI disk, bool snapImmediately = false, bool keepPosition = false)
    {
        int index = disks.Count;

        RectTransform pillarRT = (topAnchor != null) ? topAnchor : (RectTransform)transform;
        RectTransform diskRT = disk.GetComponent<RectTransform>();

        // Сделаем диск дочерним столба
        diskRT.SetParent(pillarRT, worldPositionStays: false);
        diskRT.localScale = Vector3.one;

        // Рассчитываем позицию только если нужно
        if (!keepPosition)
        {
            float step = diskSpacing;
            if (step <= 0f)
                step = diskRT.rect.height > 0f ? diskRT.rect.height : 30f;

            // Считаем Y с учётом pivot диска
            float anchoredY = baseYPosition + step * index;

            diskRT.anchoredPosition = new Vector2(0f, anchoredY);

            // Визуально верхний диск поверх нижних
            diskRT.SetAsLastSibling();
        }

        disks.Push(disk);

        disk.currentPillar = this;
        disk.lastValidPosition = diskRT.position;
    }



    // Возвращает мировую позицию верхнего места (для UI -> world conversion)
    public Vector3 GetTopPositionForDisk(DiskUI disk)
    {
        RectTransform pillarRT = (topAnchor != null) ? topAnchor : (RectTransform)transform;
        int index = disks.Count;
        float step = diskSpacing;
        if (step <= 0f)
        {
            step = disk.GetComponent<RectTransform>().rect.height > 0f ? disk.GetComponent<RectTransform>().rect.height : 30f;
        }
        Vector2 anchored = new Vector2(0f, baseYPosition + step * index);
        // Convert anchored/local to world
        Vector3 world = pillarRT.TransformPoint(anchored);
        return world;
    }

    public bool CanPlaceDisk(DiskUI disk)
    {
        DiskUI top = PeekTopDisk();
        if (top == null) return true;
        return disk.diskSize < top.diskSize;
    }

    // Поиск пега под экранной точкой (использует RectangleContainsScreenPoint)
    public static PillarUI FindPillarUnderScreenPoint(Vector2 screenPoint, Canvas canvas)
    {
        // Сначала ищем пег, чей rect содержит экранную точку
        foreach (var p in all)
        {
            RectTransform rt = p.GetComponent<RectTransform>();
            if (rt == null) continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPoint, canvas.worldCamera))
                return p;
        }
        // fallback — ближайший по X (в world coords)
        PillarUI best = null;
        float bestDist = float.PositiveInfinity;
        foreach (var p in all)
        {
            float dx = Mathf.Abs(p.GetComponent<RectTransform>().position.x - screenPoint.x);
            if (dx < bestDist) { bestDist = dx; best = p; }
        }
        return best;
    }
}
