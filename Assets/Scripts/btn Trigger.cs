using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnTrigger : MonoBehaviour
{
    public GameObject btn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            btn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            btn.SetActive(false);
        }
    }
}
