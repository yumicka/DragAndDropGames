using UnityEngine;

public class FlayingObjectManager : MonoBehaviour
{
    public void DestroyAllFlyingObjects()
    {
        FlyingObjectControllerScript[] flyingObjects =
            Object.FindObjectsByType<FlyingObjectControllerScript>(
                FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (FlyingObjectControllerScript obj in flyingObjects)
        {
            if (obj == null)
                continue;

            if (obj.CompareTag("Bomb"))
            {
                obj.TriggerExplosion();

            }
            else
            {
                obj.StartToDestroy();
            }
        }
    }
}