using System.Collections.Generic;
using UnityEngine;

public class Cars_Placement_Script : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public ObjectScript objectScr;
    public Screen_boundaries_script screenBou;

    void Start()
    {
        carPrefabs = Resources.LoadAll<GameObject>("Prefabs_cars");

        objectScr = FindFirstObjectByType<ObjectScript>();
        screenBou = FindFirstObjectByType<Screen_boundaries_script>();

        if (carPrefabs.Length == 0)
        {
            return;
        }

        SpawnRandomCars();
    }

    void SpawnRandomCars()
    {
        if (screenBou == null)
        {
            return;
        }

        Canvas parentCanvas = FindFirstObjectByType<Canvas>();
        if (parentCanvas == null)
        {
            return;
        }

        List<GameObject> spawnedCars = new List<GameObject>();

        
        Rect world = screenBou.worldBounds;

        for (int i = 0; i < carPrefabs.Length; i++)
        {
            GameObject carPrefab = carPrefabs[i];
            GameObject newCar = Instantiate(carPrefab);
            newCar.transform.SetParent(parentCanvas.transform, false);


            float padding = 100f;
            float x = Random.Range(world.xMin + padding, world.xMax - padding);
            float y = Random.Range(world.yMin + padding, world.yMax - padding);

            RectTransform rect = newCar.GetComponent<RectTransform>();
            if (rect != null)
                rect.anchoredPosition = new Vector2(x, y); // UI
            else
                newCar.transform.position = new Vector3(x, y, 0f);

            float randomScale = Random.Range(0.8f, 1.2f);
            newCar.transform.localScale = new Vector3(randomScale, randomScale, 1f);


            float smallTilt = Random.Range(-10f, 10f);
            newCar.transform.rotation = Quaternion.Euler(0f, 0f, smallTilt);

            if (newCar.GetComponent<CanvasGroup>() == null)
                newCar.AddComponent<CanvasGroup>();

            DragAndDropScript drag = newCar.GetComponent<DragAndDropScript>();
            if (drag != null)
            {
                drag.objectScr = objectScr;
                drag.screenBou = screenBou;
            }

            DropPlaceScript drop = newCar.GetComponent<DropPlaceScript>();
            if (drop != null)
            {
                drop.objScript = objectScr;
            }

            Transform winPanel = parentCanvas.transform.Find("SpawnPoint");
            if (winPanel != null)
            {
                newCar.transform.SetSiblingIndex(winPanel.GetSiblingIndex());
            }
            else
            {
                newCar.transform.SetAsFirstSibling();
            }

            spawnedCars.Add(newCar);
        }

        if (objectScr != null)
        {
            objectScr.vehicles = spawnedCars.ToArray();
            objectScr.InitializeVehicles();
        }
    }
}
