using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes_change : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("city_scene");
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("main_page");
    }

    public void HanojaScene()
    {
        SceneManager.LoadScene("Hanoja");
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
