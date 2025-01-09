using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Transform endCanva;
    int chestCount = 0;
    public int level;

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
        if(level == 1)
        {
            if (chestCount == 2)
            {
               NextLevel();
            }
        }

        if (level == 2)
        {
            if (chestCount == 1)
            {
                Victory();
            }
        }
        
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("SecondScene");
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
