using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes_change : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("city_scene");
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
