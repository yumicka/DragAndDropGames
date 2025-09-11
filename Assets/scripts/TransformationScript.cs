using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    public ObjectScript objectScript;

    void Update()
    {
        if(objectScript.lastDragged != null){
            if(Input.GetKey(KeyCode.Z)) { 
                objectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(0, 0, Time.deltaTime * 15f);
            }

            if(Input.GetKey(KeyCode.X))
            {
                objectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(0, 0, -Time.deltaTime * 15f);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y < 0.99f)
                {
                    objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                        objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y + 0.005f,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y > 0.3f)
                {
                    objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                            objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                            objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y - 0.005f,
                            1f);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x < 1.3f)
                {
                    objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x + 0.001f,
                        objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x > 0.5f)
                {
                    objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x - 0.001f,
                        objectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }
        }
    }
}
