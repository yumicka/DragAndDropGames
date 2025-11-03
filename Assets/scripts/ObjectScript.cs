using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;
    [HideInInspector]
    public Vector2[] startCoordinates;
    public Canvas can;
    public AudioSource effects;
    public AudioClip[] audioCli;
    [HideInInspector]
    public bool rightPlace = false;
    public static GameObject lastDragged = null;
    public static bool drag = false;
    public int win = 0;
    public GameObject winPanel;


    public void InitializeVehicles()
    {
        if (vehicles == null || vehicles.Length == 0)
        {
   
            return;
        }

        vehicles = vehicles
        .Where(v => !v.name.ToLower().Contains("place"))
        .ToArray();

        if (vehicles.Length == 0)
        {
            return;
        }

       
        startCoordinates = new Vector2[vehicles.Length];
        for (int i = 0; i < vehicles.Length; i++)
        {
            RectTransform rect = vehicles[i].GetComponent<RectTransform>();
            if (rect != null)
            {
                startCoordinates[i] = rect.localPosition;
            }
          
        }
    }
}

