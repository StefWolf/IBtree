using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartTheGame() {
        SceneManager.LoadScene("MainScene");
    }

    public void Exit() {
        Application.Quit();
    }
}
