using UnityEngine;
using UnityEngine.EventSystems;

public class TransformationScript : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public float scaleSpeed = 0.5f;
    public static bool isTransforming = false;

    private bool rotateCW, rotateCCW, scaleUpY, scaleDownY, scaleUpX, scaleDownX;

    void Update()
    {
        if (ObjectScript.lastDragged == null)
            return;

        RectTransform rt = ObjectScript.lastDragged.GetComponent<RectTransform>();
        if (rt == null) return;

        if (rotateCW)
            rt.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        if (rotateCCW)
            rt.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (scaleUpY && rt.localScale.y < 1.4f)
            rt.localScale += new Vector3(0, scaleSpeed * Time.deltaTime * 6f, 0);

        if (scaleDownY && rt.localScale.y > 0.2f)
            rt.localScale -= new Vector3(0, scaleSpeed * Time.deltaTime * 6f, 0);

        if (scaleUpX && rt.localScale.x < 1.4f)
            rt.localScale += new Vector3(scaleSpeed * Time.deltaTime * 6f, 0, 0);

        if (scaleDownX && rt.localScale.x > 0.2f)
            rt.localScale -= new Vector3(scaleSpeed * Time.deltaTime * 6f, 0, 0);

        isTransforming = rotateCW || rotateCCW || scaleUpY || scaleDownY || scaleUpX || scaleDownX;
    }

    // Варианты с BaseEventData (чтоб ничего не упало, если где-то ещё висят)
    public void StartRotateCW(BaseEventData _) { StartRotateCW(); }
    public void StopRotateCW(BaseEventData _) { StopRotateCW(); }

    public void StartRotateCCW(BaseEventData _) { StartRotateCCW(); }
    public void StopRotateCCW(BaseEventData _) { StopRotateCCW(); }

    public void StartScaleUpY(BaseEventData _) { StartScaleUpY(); }
    public void StopScaleUpY(BaseEventData _) { StopScaleUpY(); }

    public void StartScaleDownY(BaseEventData _) { StartScaleDownY(); }
    public void StopScaleDownY(BaseEventData _) { StopScaleDownY(); }

    public void StartScaleUpX(BaseEventData _) { StartScaleUpX(); }
    public void StopScaleUpX(BaseEventData _) { StopScaleUpX(); }

    public void StartScaleDownX(BaseEventData _) { StartScaleDownX(); }
    public void StopScaleDownX(BaseEventData _) { StopScaleDownX(); }

    // Основные методы без параметров
    public void StartRotateCW() { rotateCW = true; }
    public void StopRotateCW() { rotateCW = false; }

    public void StartRotateCCW() { rotateCCW = true; }
    public void StopRotateCCW() { rotateCCW = false; }

    public void StartScaleUpY() { scaleUpY = true; }
    public void StopScaleUpY() { scaleUpY = false; }

    public void StartScaleDownY() { scaleDownY = true; }
    public void StopScaleDownY() { scaleDownY = false; }

    public void StartScaleUpX() { scaleUpX = true; }
    public void StopScaleUpX() { scaleUpX = false; }

    public void StartScaleDownX() { scaleDownX = true; }
    public void StopScaleDownX() { scaleDownX = false; }
}
