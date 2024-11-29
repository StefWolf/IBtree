using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Transform endCanva;
    int chestCount = 0;

    private void Start()
    {
        Time.timeScale = 1f;
        endCanva = FindFirstObjectByType<PlayerMovement>().gameObject.transform.GetChild(1).GetChild(2);
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        endCanva.GetChild(1).gameObject.SetActive(true);
        endCanva.gameObject.SetActive(true);
    }

    public void SetChestCount()
    {
        chestCount++;
        if(chestCount == 2)
        {
            Victory();
        }
    }

    public void Victory()
    {
        Time.timeScale = 0f;
        endCanva.GetChild(0).gameObject.SetActive(true);
        endCanva.gameObject.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
