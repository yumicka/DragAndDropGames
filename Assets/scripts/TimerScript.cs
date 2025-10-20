using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject[] stars;
    public float elapsedTime;
    public bool isRunning = true;
    public int starCount = 0; 

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopAndEvaluateStars()
    {
        isRunning = false;

        if (elapsedTime <= 120f) 
            starCount = 3;
        else if (elapsedTime <= 180f) 
            starCount = 2;
        else
            starCount = 1;

        ShowStars();
    }

    private void ShowStars()
    {
        if (stars == null || stars.Length == 0) return;

        foreach (var s in stars)
            s.SetActive(false);

        for (int i = 0; i < starCount && i < stars.Length; i++)
            stars[i].SetActive(true);
    }
}
