using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string gameSceneName; 

    public void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }





    public void QuitGame()
    {
        Debug.Log("QUITGAME");
        Application.Quit();
    }

}