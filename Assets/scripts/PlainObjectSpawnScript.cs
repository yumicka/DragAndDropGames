using UnityEngine;

public class PlainObjectSpawnScript : MonoBehaviour
{
    Screen_boundaries_script screenBoundriesScript;
    public GameObject[] cloudsPrefabs;
    public GameObject[] objectPrefabs;
    public Transform spawnPoint;
    public float cloudSpawnInterval = 2f;
    public float objectSpawnInterval = 3f;
    private float minY, maxY;
    public float cloudMinSpeed = 1.5f;
    public float cloudMaxSpeed = 2f;
    public float objectMaxSpeed = 200f;
    public float objectMinSpeed = 2f;




    void Start()
    {
        screenBoundriesScript = FindFirstObjectByType<Screen_boundaries_script>();
        minY = screenBoundriesScript.minY;
        maxY = screenBoundriesScript.maxY;
        InvokeRepeating(nameof(SpawnCloud), 0f, cloudSpawnInterval);
        InvokeRepeating(nameof(SpawnObject), 0f, objectSpawnInterval);
    }

    void SpawnCloud()
    {
        if (cloudsPrefabs.Length == 0)
            return;
        GameObject cloudPrefab = cloudsPrefabs[Random.Range(0, cloudsPrefabs.Length)];
        float y = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(
            spawnPoint.position.x, y, spawnPoint.position.z);
        GameObject cloud =
            Instantiate(cloudPrefab, spawnPosition, Quaternion.identity, spawnPoint);
        float movementSpeed = Random.Range(cloudMinSpeed, cloudMaxSpeed);
        FlyingObjectControllerScript controller = 
            cloud.GetComponent<FlyingObjectControllerScript>();
        controller.speed = movementSpeed;

    }

    void SpawnObject()
    {
       if (objectPrefabs.Length == 0)
            return;

        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
        float y = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(-
            spawnPoint.position.x, y, spawnPoint.position.z);
        GameObject flyingObject =
            Instantiate(objectPrefab, spawnPosition, Quaternion.identity, spawnPoint);
        float movementSpeed = Random.Range(objectMinSpeed, objectMaxSpeed);
        FlyingObjectControllerScript controller =
            flyingObject.GetComponent<FlyingObjectControllerScript>();
        controller.speed = -movementSpeed;
    }

}
