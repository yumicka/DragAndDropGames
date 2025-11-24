using System.Collections.Generic;
using UnityEngine;

public class GameManagerUI : MonoBehaviour
{
    public PillarUI[] pillars;
    public GameObject diskPrefabUI; // UI prefab (Image + DiskUI + CanvasGroup)
    public int diskCount = 4;

    public DiskUI[] disks;

    private List<DiskUI> created = new List<DiskUI>();

    void Start()
    {
        if (pillars == null || pillars.Length == 0)
            Debug.LogError("Pillars not set");
        if (diskPrefabUI == null)
            Debug.LogError("diskPrefabUI not set");

        
        if (disks != null && disks.Length > 0)
        {
            System.Array.Sort(disks, (a, b) => b.diskSize.CompareTo(a.diskSize));


            foreach (var disk in disks)
            {
                pillars[0].PushDisk(disk, snapImmediately: true);
            }
        }
        else
        {
            // Если дисков нет вручную, можно создать их автоматически
            for (int i = diskCount; i >= 1; i--)
            {
                GameObject diskGO = Instantiate(diskPrefabUI, pillars[0].transform);
                DiskUI diskUI = diskGO.GetComponent<DiskUI>();
                diskUI.diskSize = i;
                pillars[0].PushDisk(diskUI, snapImmediately: true);
                created.Add(diskUI);
            }
        }
    }
}
