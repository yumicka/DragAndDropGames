using System.Collections.Generic;
using UnityEngine;

public class Cars_Placement_Script : MonoBehaviour
{
    public GameObject[] carPrefabs; // Префабы из Resources/Prefabs_cars
    public ObjectScript objectScr;
    public Screen_boundaries_script screenBou;

    [Header("Настройки спавна")]
    public int carsToSpawn = 5; // Количество машин
    public Vector2 spawnAreaMin = new Vector2(-5f, -3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 3f);

    void Start()
    {
        // Загружаем все префабы машин
        carPrefabs = Resources.LoadAll<GameObject>("Prefabs_cars");

        objectScr = FindFirstObjectByType<ObjectScript>();
        screenBou = FindFirstObjectByType<Screen_boundaries_script>();

        Debug.Log("Найдено префабов машин: " + carPrefabs.Length);

        if (carPrefabs.Length == 0)
        {
            Debug.LogError("? Не найдены префабы машин в папке Resources/Prefabs_cars");
            return;
        }

        SpawnRandomCars();
    }

    void SpawnRandomCars()
    {
        if (screenBou == null)
        {
            Debug.LogError("? Screen_boundaries_script не найден!");
            return;
        }

        Canvas parentCanvas = FindFirstObjectByType<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("? Canvas не найден в сцене!");
            return;
        }

        // Список всех заспавненных машин
        List<GameObject> spawnedCars = new List<GameObject>();

        // ?? Создаём машины
        for (int i = 0; i < carPrefabs.Length; i++)
        {
            GameObject carPrefab = carPrefabs[i];
            GameObject newCar = Instantiate(carPrefab);

            newCar.transform.SetParent(parentCanvas.transform, false);

            // Генерация случайной позиции внутри границ экрана
            float x = Random.Range(screenBou.minX, screenBou.maxX);
            float y = Random.Range(screenBou.minY, screenBou.maxY);

            RectTransform rect = newCar.GetComponent<RectTransform>();
            if (rect != null)
                rect.anchoredPosition = new Vector2(x, y); // UI
            else
                newCar.transform.position = new Vector3(x, y, 0f); // 2D/3D

            // Добавляем CanvasGroup, если нет
            if (newCar.GetComponent<CanvasGroup>() == null)
                newCar.AddComponent<CanvasGroup>();

            // Настраиваем DragAndDrop
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

            // ?? Размещение по иерархии (до SpawnPoint, если он есть)
            Transform winPanel = parentCanvas.transform.Find("SpawnPoint");
            if (winPanel != null)
            {
                newCar.transform.SetSiblingIndex(winPanel.GetSiblingIndex());
            }
            else
            {
                newCar.transform.SetAsFirstSibling();
            }

            spawnedCars.Add(newCar); // добавляем в список
        }

        // ?? Передаём список машин в ObjectScript
        if (objectScr != null)
        {
            objectScr.vehicles = spawnedCars.ToArray();
            objectScr.InitializeVehicles();
            Debug.Log($"? Машин передано в ObjectScript: {objectScr.vehicles.Length}");
        }
        else
        {
            Debug.LogWarning("?? ObjectScript не найден, не могу передать список машин.");
        }
    }
}
