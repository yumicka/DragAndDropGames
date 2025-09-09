using UnityEngine;

public class Screen_boundaries_script : MonoBehaviour
{
    [HideInInspector]
    public Vector3 screenPoint, offset;
    private float minX, maxX, minY, maxY;
    public float padding = 0.02f;

    void Awake() //atrak par start
    {
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(Vector3.zero); //viss ir pa nullem
        Vector3 upperRight =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float widthReduction = (upperRight.x - lowerLeft.x) * padding;
        float heightReduction = (upperRight.y - lowerLeft.y) * padding;
        minX = lowerLeft.x + widthReduction;
        maxX = upperRight.x - widthReduction;
        minY = lowerLeft.y + heightReduction;
        maxX = upperRight.y - heightReduction;
    }

    public Vector2 GetClampedPosition(Vector3 position)
    {
        return new Vector2(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY)
        );
    }
}
