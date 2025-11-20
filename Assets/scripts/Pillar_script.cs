using System.Collections.Generic;
using UnityEngine;

public class Pillar_script : MonoBehaviour
{
    private Stack<Disk_drag_drop> disks = new Stack<Disk_drag_drop>();
    public float diskHeight = 0.3f; // ???????? ?? Y ??? ??????

    private static List<Pillar_script> allPegs = new List<Pillar_script>();

    void Awake()
    {
        allPegs.Add(this);
    }
    void OnDestroy()
    {
        allPegs.Remove(this);
    }

    public Disk_drag_drop PeekTopDisk()
    {
        if (disks.Count == 0) return null;
        return disks.Peek();
    }

    public Disk_drag_drop PopTopDisk()
    {
        if (disks.Count == 0) return null;
        return disks.Pop();
    }

    public void PushDisk(Disk_drag_drop disk)
    {
        disks.Push(disk);
        Vector3 pos = GetTopPositionForDisk(disk);
        disk.transform.position = pos;
        disk.transform.rotation = Quaternion.identity;
        disk.currentPeg = this;
        disk.lastValidPosition = pos;
        disk.rb.bodyType = RigidbodyType2D.Kinematic; // ????????? ???? ????? ???????
    }

    public Vector3 GetTopPositionForDisk(Disk_drag_drop disk)
    {
        Vector3 basePos = transform.position;
        float y = basePos.y + diskHeight * disks.Count;
        return new Vector3(basePos.x, y, basePos.z);
    }

    public bool CanPlaceDisk(Disk_drag_drop disk)
    {
        Disk_drag_drop top = PeekTopDisk();
        if (top == null) return true;
        return disk.diskSize < top.diskSize;
    }

    public int DiskCount => disks.Count;

    public static Pillar_script FindClosestPeg(Vector3 pos)
    {
        Pillar_script best = null;
        float bestDist = float.PositiveInfinity;
        foreach (var p in allPegs)
        {
            float d = Mathf.Abs(p.transform.position.x - pos.x);
            if (d < bestDist)
            {
                bestDist = d;
                best = p;
            }
        }
        return best;
    }
}
