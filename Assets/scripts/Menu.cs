using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadScene(int x)
    {
        switch (x)
        {
            case 1:
                SceneManager.LoadScene("Intersection 1", LoadSceneMode.Single);
                break;
            case 2:
                SceneManager.LoadScene("Intersection 2", LoadSceneMode.Single);
                break;
            case 3:
                SceneManager.LoadScene("Intersection 3", LoadSceneMode.Single);
                break;
        }
    }


    public void close()
    {
        Debug.Log("Zamykanie aplikacji...");
        Application.Quit();
    }
}
