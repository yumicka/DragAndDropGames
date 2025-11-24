using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PillarUI : MonoBehaviour
{
    private Stack<DiskUI> disks = new Stack<DiskUI>();

    public float diskSpacing = 50f;
    public float baseYPosition = 0f;
    public RectTransform topAnchor;

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
        if (diskSpacing <= 0f)
        {
            DiskUI any = GetComponentInChildren<DiskUI>();
            if (any != null)
            {
                RectTransform dr = any.GetComponent<RectTransform>();
                diskSpacing = dr.rect.height;
            }
            if (diskSpacing <= 0f) diskSpacing = 30f;
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

    public int DiskCount => disks.Count;

    public void PushDisk(DiskUI disk, bool snapImmediately = false, bool keepPosition = false)
    {
        int index = disks.Count;

        RectTransform pillarRT = (topAnchor != null) ? topAnchor : (RectTransform)transform;
        RectTransform diskRT = disk.GetComponent<RectTransform>();

        diskRT.SetParent(pillarRT, worldPositionStays: false);
        diskRT.localScale = Vector3.one;

        if (!keepPosition)
        {
            float step = diskSpacing;
            if (step <= 0f)
                step = diskRT.rect.height > 0f ? diskRT.rect.height : 30f;

            float anchoredY = baseYPosition + step * index;
            diskRT.anchoredPosition = new Vector2(0f, anchoredY);
            diskRT.SetAsLastSibling();
        }

        disks.Push(disk);
        disk.currentPillar = this;
        disk.lastValidPosition = diskRT.position;

        Debug.Log($"PushDisk: {disk.name} -> {name}, index={index}");
    }

    // 🔹 НОВОЕ: найти столб по диску
    public static PillarUI FindPillarContainingDisk(DiskUI disk)
    {
        foreach (var p in all)
        {
            foreach (var d in p.disks)
            {
                if (d == disk) return p;
            }
        }
        return null;
    }

    public bool CanPlaceDisk(DiskUI disk)
    {
        DiskUI top = PeekTopDisk();
        if (top == null) return true;
        return disk.diskSize < top.diskSize;
    }

    public static PillarUI FindPillarUnderScreenPoint(Vector2 screenPoint, Canvas canvas)
    {
        foreach (var p in all)
        {
            RectTransform rt = p.GetComponent<RectTransform>();
            if (rt == null) continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, screenPoint, canvas.worldCamera))
                return p;
        }

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
