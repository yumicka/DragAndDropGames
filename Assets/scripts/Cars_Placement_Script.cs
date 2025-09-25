using UnityEngine;

public class Cars_Placement_Script : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public ObjectScript objectScr;
    public Screen_boundaries_script screenBou;

    void Awake()
    {
        carPrefabs = Resources.LoadAll<GameObject>("Prefabs_cars");
        objectScr = FindFirstObjectByType<ObjectScript>();
        screenBou = FindFirstObjectByType<Screen_boundaries_script>();
        foreach (GameObject carPrefab in carPrefabs)
        {
            DragAndDropScript dragAndDrop = carPrefab.GetComponent<DragAndDropScript>();
            if (dragAndDrop != null)
            {
                dragAndDrop.objectScr = objectScr;
                dragAndDrop.screenBou = screenBou;
            }
        }

        // massivs emptyiem
        Debug.Log(carPrefabs.Length);
    }

    void Update()
    {
        
    }
}
