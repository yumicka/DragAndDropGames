using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    public PillarUI[] pillars; // ??????? ? ?????????? (3)
    public GameObject diskPrefabUI; // UI prefab (Image + DiskUI + CanvasGroup)
    public int diskCount = 4;

    private List<DiskUI> created = new List<DiskUI>();

    void Start()
    {
        if (pillars == null || pillars.Length == 0) Debug.LogError("Pillars not set");
        if (diskPrefabUI == null) Debug.LogError("diskPrefabUI not set");
    }

    
}
