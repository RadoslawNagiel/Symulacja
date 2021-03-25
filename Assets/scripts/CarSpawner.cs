using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public float minSpawnDelay = 5f;
    public float maxSpawnDelay = 10f;
    public UIScript uiScript;
    public GameObject[] points;
    public Blocker[] blockerScript;
    private float[] delay;

    void Start()
    {
        delay = new float[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            delay[i] = (int)Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void Update()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (delay[i] <= 0 && !blockerScript[i].car)
            {
                GameObject Created = new GameObject();
                switch(Random.Range(1, 6))
                {
                    case 1:
                        Created = (GameObject)Instantiate(Resources.Load("carAI 1"));
                        break;
                    case 2:
                        Created = (GameObject)Instantiate(Resources.Load("carAI 2"));
                        break;
                    case 3:
                        Created = (GameObject)Instantiate(Resources.Load("carAI 3"));
                        break;
                    case 4:
                        Created = (GameObject)Instantiate(Resources.Load("carAI 4"));
                        break;
                    case 5:
                        Created = (GameObject)Instantiate(Resources.Load("carAI 5"));
                        break;
                }
                Created.transform.position = points[i].transform.position;
                AIMap aim = points[i].GetComponent<AIMap>();
                int r = Random.Range(0, aim.nextPoint.Length);
                Created.GetComponent<AICar>().target = aim.nextPoint[r];
                Created.GetComponent<AICar>().uiScript = uiScript;
                Vector3 relativeVector = Created.transform.InverseTransformPoint(aim.nextPoint[r].position);
                float newSteer = (relativeVector.x / relativeVector.magnitude) * 90;
                if (relativeVector.z == 0)
                {
                    if (relativeVector.x > 0)
                        newSteer = 90;
                    else
                        newSteer = -90;
                }
                else if (relativeVector.x == 0)
                {
                    if (relativeVector.z > 0)
                        newSteer = 0;
                    else
                        newSteer = 180;
                }
                else
                    newSteer = Mathf.Atan(relativeVector.x / relativeVector.z) * (180 / Mathf.PI);
                Created.transform.Rotate(0, newSteer, 0);
                delay[i] = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
            else
            {
                delay[i] -= Time.deltaTime;
            }
        }
    }
}
