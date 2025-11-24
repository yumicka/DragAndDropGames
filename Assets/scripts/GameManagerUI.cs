using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public PillarUI[] pillars;
    public PillarUI startPillar;     
    public PillarUI goalPillar;
    public GameObject diskPrefabUI;
    public int totalDisks = 6;
    private int moves = 0;

    public Text movesText;           
    public GameObject winPanel;

    public DiskUI[] disks;

    private List<DiskUI> created = new List<DiskUI>();

    void Start()
    {
        if (pillars == null || pillars.Length == 0)
            Debug.LogError("Pillars not set");
        if (diskPrefabUI == null)
            Debug.LogError("diskPrefabUI not set");


        if (winPanel != null)
            winPanel.SetActive(false);

        moves = 0;
        UpdateMovesText();

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
            for (int i = totalDisks; i >= 1; i--)
            {
                GameObject diskGO = Instantiate(diskPrefabUI, pillars[0].transform);
                DiskUI diskUI = diskGO.GetComponent<DiskUI>();
                diskUI.diskSize = i;
                pillars[0].PushDisk(diskUI, snapImmediately: true);
                created.Add(diskUI);
            }
        }
    }

    public void OnDiskPlaced(PillarUI toPillar)
    {
        moves++;
        UpdateMovesText();

        if (toPillar == goalPillar && goalPillar.DiskCount == totalDisks)
        {
            Debug.Log("WIN! 🏆 Башня собрана!");
            ShowWinPanel();
        }
    }

    private void UpdateMovesText()
    {
        if (movesText != null)
            movesText.text = "Soļi: " + moves;
    }

    public void ShowWinPanel()
    {
        Debug.Log($"WIN! Башня собрана за {moves} ходов");

        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
