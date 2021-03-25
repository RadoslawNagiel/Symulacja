using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public bool car = false;
    public Transform[] goal; 
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="AICar")
        {
            bool check = false;
            if (goal.Length == 0)
            {
                check = true;
            }
            else 
                for (int i = 0; i < goal.Length; i++)
                {
                    if (other.gameObject.GetComponentInParent<AICar>().target == goal[i])
                        check = true;
                }
            if(check)
            {
                car = true;
                if (gameObject.transform.childCount > 0)
                    gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AICar")
        {
            car = false;
            if (gameObject.transform.childCount > 0)
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
