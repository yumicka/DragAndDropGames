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
            Debug.LogError("?? Не найдены префабы машин в папке Resources/Prefabs_cars");
            return;
        }

        SpawnRandomCars();
    }

    void SpawnRandomCars()
    {
        if (screenBou == null)
        {
            Debug.LogError("Screen_boundaries_script не найден!");
            return;
        }

        //Canvas parentCanvas = FindFirstObjectByType<Canvas>();

        for (int i = 0; i < carPrefabs.Length; i++)
        {
            GameObject carPrefab = carPrefabs[i]; // берем конкретный префаб
            GameObject newCar = Instantiate(carPrefab);

            // Помещаем в Canvas, если это UI
            Canvas parentCanvas = FindFirstObjectByType<Canvas>();
            if (parentCanvas != null)
                newCar.transform.SetParent(parentCanvas.transform, false);

            // Генерация случайной позиции внутри границ камеры
            float x = Random.Range(screenBou.minX, screenBou.maxX);
            float y = Random.Range(screenBou.minY, screenBou.maxY);
            Vector3 spawnPos = new Vector3(x, y, 0f);

            RectTransform rect = newCar.GetComponent<RectTransform>();
            if (rect != null)
                rect.anchoredPosition = new Vector2(x, y); // для UI
            else
                newCar.transform.position = spawnPos; // для 2D/3D

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
                drop.objScript = objectScr; // правильно назначаем
            }

            newCar.transform.SetAsLastSibling(); // на передний план
        }

    }

}
