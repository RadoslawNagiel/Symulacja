using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour {

    public GameObject[] trafficLights;
    [Tooltip("1-red 2-yellow 3-green 4-yellow+red")] public string[] status;
    public float[] statusDelay;

    private float delay = 0;
    private int index = -1;

    private void FixedUpdate()
    {
        if(delay <= 0)
        {
            index++;
            if (index == statusDelay.Length)
                index = 0;
            delay = statusDelay[index];
            for (int i = 0; i < trafficLights.Length; i++)
            {
                ChangeLight(i, status[index][i] - '0');
            }
        }
        else
        {
            delay -= Time.deltaTime;
        }
    }

    void ChangeLight(int id, int stat)
    {
        for (int i = 0; i < trafficLights[id].transform.childCount; i++)
        {
            trafficLights[id].transform.GetChild(i).gameObject.SetActive(false);
        }

        switch (stat)
        {
            case 1:
                trafficLights[id].transform.Find("Red").gameObject.SetActive(true);
                break;
            case 2:
                trafficLights[id].transform.Find("Yellow").gameObject.SetActive(true);
                break;
            case 3:
                trafficLights[id].transform.Find("Green").gameObject.SetActive(true);
                break;
            case 4:
                trafficLights[id].transform.Find("Red").gameObject.SetActive(true);
                trafficLights[id].transform.Find("Yellow").gameObject.SetActive(true);
                break;
        }
    }

}
