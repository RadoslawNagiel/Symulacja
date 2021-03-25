using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text StartButtonText;
    public Text infoText;
    public Text capacityText;
    public Scrollbar scrollbar;
    public GameObject[] cameras;

    bool start = false;
    float speed = 1;
    int cam = 0;

    int[] cars = new int[10];
    int index = 0;
    float counterdelay = 1f;
    ulong allcars = 0;
    int time = 0;
    float delay = 60;

    private void Start()
    {
        StartButtonText.text = "Start";
        Time.timeScale = 0;
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }
        cameras[0].SetActive(true);
    }

    private void FixedUpdate()
    {
        if (counterdelay <= 0)
        {
            counterdelay = 6f;

            int amountCars = 0;
            for (int i = 0; i < 10; i++)
            {
                amountCars += cars[i];
            }
            capacityText.text = "Przepustowość:\n" + amountCars + " / min.";

            if (delay<=0)
            {
                allcars += (ulong)(cars[index] * 10);
                time++;
                capacityText.text += "\nŚrednia: " + allcars / (ulong)time;
            }

            index = index == 9 ? 0 : index + 1;
            cars[index] = 0;
        }
        else
        {
            counterdelay -= Time.deltaTime;
        }
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
    }

    public void close()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void StartClick()
    {
        if (!start)
        {
            StartButtonText.text = "Pauza";
            Time.timeScale = speed;
            start = true;
        }
        else
        {
            StartButtonText.text = "Start";
            Time.timeScale = 0;
            start = false;
        }
    }

    public void ScrollCheck()
    {
        speed = 1 + scrollbar.value * 9;
        if(start)
            Time.timeScale = speed;
        infoText.text = speed.ToString();
    }

    public void CameraChange()
    {
        cameras[cam].SetActive(false);
        cam = cam+1 < cameras.Length ? cam + 1 : 0;
        cameras[cam].SetActive(true);
    }

    public void addCar()
    {
        cars[index]++;
    }
}
