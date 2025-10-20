using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.LogError("?? Vehicles array is empty! Ќазначь машины перед инициализацией.");
            return;
        }

        startCoordinates = new Vector2[vehicles.Length];
        for (int i = 0; i < vehicles.Length; i++)
        {
            startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;
            Debug.Log($"? Vehicle {i} start position: {startCoordinates[i]}");
        }
    }

}